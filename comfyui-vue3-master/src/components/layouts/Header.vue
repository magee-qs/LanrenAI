<template>
    <div class="header">
        <div class="logo" @click="toHome()">
            <img src="/static/logo/banner2.png" height="48px" />
        </div>
        <div class="menu">
            <el-space direction="horizontal" :size="0">
                <div class="item" @click="selectMenu('/')">
                    首 &nbsp;&nbsp;页
                </div>
                <div class="item" @click="selectMenu('/work')">
                    我的作品
                </div>
                <!-- <div class="item" @click="selectMenu('/')">
                    我的收藏
                </div> -->
            </el-space>
        </div>
        <div class="user">
            <!-- <div class="user-item">
                <el-badge is-dot class="item" :value="msgCount" :max="99">
                    <el-button class="share-button" icon="ChatDotRound" type="primary" />
                </el-badge>
            </div> -->
            <div class="user-item" v-if="userStore.cost > 0">
                <el-button type="primary" icon="StarFilled"> {{ userStore.cost }} </el-button>
            </div>

            <div class="user-item" style="margin-right: 20px;" v-if="userStore.token">
                <el-dropdown size="large" type="primary">
                    <div class="user-name">
                        <el-space direction="horizontal">
                            <el-avatar src="/static/avatar/avatar4.webp"></el-avatar>
                            <el-text>{{ userStore.userInfo.userName }}</el-text>
                        </el-space>
                    </div>
                    <template #dropdown>
                        <el-dropdown-menu>
                            <el-dropdown-item @click="menuClick('/user')">
                                <el-space direction="horizontal">
                                    <el-icon>
                                        <User />
                                    </el-icon>
                                    个人中心
                                </el-space>
                            </el-dropdown-item>
                            <el-dropdown-item @click="menuClick('/order')">
                                <el-space direction="horizontal">
                                    <el-icon>
                                        <Coin />
                                    </el-icon>
                                    充值订单
                                </el-space>
                            </el-dropdown-item>
                            <el-dropdown-item @click="menuClick('/order')">
                                <el-space direction="horizontal">
                                    <el-icon>
                                        <Setting />
                                    </el-icon>
                                    账号设置
                                </el-space>
                            </el-dropdown-item>
                            <el-dropdown-item @click="logout()">
                                <el-space direction="horizontal">
                                    <el-icon>
                                        <Delete />
                                    </el-icon>
                                    退出登录
                                </el-space>
                            </el-dropdown-item>
                        </el-dropdown-menu>
                    </template>
                </el-dropdown>
            </div>
            <div v-else style="margin-right: 20px;">
                <el-button type="primary" radius prefix-icon="layui-icon-fire" @click="toLogin">用户登录</el-button>
            </div>
        </div>

    </div>
</template>
<script lang="ts" setup>
import useUserStore from '@/stores/useUserStore';
import { onMounted, ref } from 'vue';
import router from '@/router';
import { StarFilled } from '@element-plus/icons-vue';
import message from '@/utils/message';


const userStore = useUserStore()


const selectMenu = (path: string) => {
    router.push({ path: path })
}




const menuClick = function (path: string) {
    router.push({ path: path })

}

const logout = function () {
    message.confirm('确定退出登录账号', '提示').then(() => {
        userStore.logout()
        router.push('/')
    })
}

const toLogin = function () {
    router.push('/login')
}

const toHome = function () {
    router.push('/')
}
</script>
<style scoped lang="less">
.header {
    min-width: 1200px;
    display: flex;
    flex-direction: row;
    border-bottom: 1px solid #DCDFE6;
    background-color: #fff;

    .logo {
        img {
            margin-top: 6px;
        }
    }

    .logo:hover {
        cursor: pointer;
    }

    .menu {
        line-height: 60px;
        color: #303133;
        font-size: 16px;

        .item {
            display: block;
            padding-left: 20px;
            padding-right: 20px;
        }

        .item:hover {
            cursor: pointer;
            background-color: #E4E7ED;
        }
    }

    .user {
        display: flex;
        gap: 16px;
        margin-left: auto;
        align-items: center;

        .user-name:hover {
            cursor: pointer;
        }
    }

}
</style>