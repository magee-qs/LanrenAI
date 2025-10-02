<template>
    <div class="wrapper">
        <div class="login-title"><el-text type="primary" size="large">欢迎登录</el-text></div>

        <el-form :model="form" class="login-form" ref="loginForm" label-width="100" :rules="rules">
            <el-form-item label="账  号">
                <el-input v-model="form.telephone" placeholder="手机号" />
            </el-form-item>

            <el-form-item label="验证码">
                <el-col :span="12">
                    <el-input v-model="form.verifyCode" placeholder="短信验证码"></el-input>
                </el-col>
                <el-col :span="12" style="text-align: right;">
                    <el-button @click="handlerSMSClick" style="width: 160px;" :disabled="isChecked">
                        {{ smsLabel }}</el-button>
                </el-col>
            </el-form-item>
            <el-button type="primary" @click="submit">开始创作</el-button>
        </el-form>

        <el-dialog v-model="dialogVisible" title="校验码" height="280" width="320">
            <el-space>
                <img :src="captcha" /><el-input v-model="checkCode"></el-input><el-button
                    @click="doCheckCode">继续</el-button>
            </el-space>
        </el-dialog>
    </div>

</template>
<script lang="ts" setup>
import { getCaptcha, getSMSCode, login, verifyCheckCode } from '@/api/login';
import router from '@/router';
import useUserStore from '@/stores/useUserStore';
import message from '@/utils/message';
import validate from '@/utils/validate';
import { computed, reactive, ref } from 'vue';

const userStore = useUserStore()

const form = reactive({
    telephone: '',
    verifyCode: ''
})

const captcha = ref()
const dialogVisible = ref(false)
const isChecked = ref(false)
const checkCode = ref('')
const loadCaptcha = () => {
    return getCaptcha().then(res => {
        if (res.data) {
            captcha.value = res.data
        } else {
            captcha.value = ''
        }
    })
}


const isPhoneNumber = computed(() => {
    return validate.isPhoneNumber(form.telephone)
})



const smsLabel = ref('获取短信验证码')
const doCheckCode = () => {
    if (checkCode.value.trim() == '') {
        message.error('请输入校验码')
        return
    }
    verifyCheckCode(checkCode.value).then(res => {
        let result = res as any
        if (result.success) {
            dialogVisible.value = false
            //发起验证码申请   60秒内不能点击发送短信验证码
            isChecked.value = true
            //发起验证码
            getSMSCode(form.telephone).then(() => {
                let count = 59
                let _internal = setInterval(() => {
                    if (count == 0) {
                        smsLabel.value = '获取短信验证码'
                        isChecked.value = false
                        clearInterval(_internal)
                    } else {
                        count--
                        smsLabel.value = '(' + count + ')短信验证码'
                    }
                }, 1000);
            })

        }
    })
}




const handlerSMSClick = () => {
    if (isPhoneNumber.value == false) {
        message.error('电话号码不正确')
        return
    }
    loadCaptcha().then(() => {
        dialogVisible.value = true
    })
}

const rules = ref({
    telephone: {
        validator(rule: any, value: any, callback: Function) {
            if (!validate.isPhoneNumber(value)) {
                callback(new Error('电话号码不正确'))
            } else {
                return true
            }
        }
    },
    checkCode: [{ required: true, message: '验证码不能为空', trigger: 'blur' }]
})


const loginForm = ref()

/* 用户登录 */
const submit = () => {
    loginForm.value.validate((isValidate: boolean) => {
        if (!!isValidate) {
            login(form).then(res => {
                userStore.login(res.data)

                //更新点数
                userStore.updateCost()

                //页面跳转
                router.push('/');
            })
        }
    })
}
</script>
<style lang="less" scoped>
.wrapper {
    margin: 0 auto;
    text-align: center;
}

.login-title {
    font-size: 24px;
    font-weight: bold;
    line-height: 120px;
}

.login-form {
    width: 450px;
    margin: 0 auto;
}

.smsbtn {}
</style>