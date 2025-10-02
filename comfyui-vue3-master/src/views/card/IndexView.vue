<template>
    <div class="warrper">
        <div class="list">
            <CardItem v-for="(item) in cardList" :card="item"></CardItem>
        </div>
        <div class="m-t1">
            <img src="/static/line.svg">
            <span>会员权益</span>
            <img src="/static/line.svg" style="transform: scale(-1,-1);">
        </div>

        <div class="m-t2">
            <CardDetail></CardDetail>

        </div>
        <div class="m-t3">
            ※会员每月算力按月下发 ※当月有效 ※到期后自动重置
        </div>
    </div>
</template>
<script lang="ts" setup>
import { getCardList } from '@/api/order';
import CardItem from '@/components/card/CardItem.vue';
import CardDetail from '@/components/card/CardDetail.vue';
import { onMounted, ref } from 'vue';


const cardList = ref([])

onMounted(() => {
    getCardList().then(res => {
        cardList.value = res.data
    })
})
</script>
<style lang="less" scoped>
.warrper {
    min-width: 900px;
}

.list {
    display: flex;
    flex-direction: row;
    justify-content: center;
    padding-top: 30px;

    .item+.item {
        margin-left: 20px;
    }
}

.m-t1 {
    padding-top: 50px;
    text-align: center;

    span {
        font-size: 24px;
        font-weight: 600;

        padding-left: 20px;
        padding-right: 20px;
    }

}

.m-t2 {
    display: flex;
    flex-direction: row;
    justify-content: center;
    padding-top: 50px;

    .item+.item {
        border-left: none;
    }
}

.m-t3 {
    padding-top: 16px;
    font-size: 14px;
    text-align: center;
    color: #303133;
    padding-bottom: 16px;
}

.item-title {
    width: 300px;
    border: 1px solid #d2d2d2;

    #header {
        line-height: 60px;
        font-size: 18px;
        font-weight: 500;
        background-color: #FAFAFA;

        img {
            display: inline-block;
            vertical-align: middle;
        }

        span {
            display: inline-block;
            vertical-align: middle;
        }

    }

    .row1 {
        background-color: #FFFFFF;
        font-size: 12px;
    }

    .row2 {
        background-color: #FAFAFA;
        line-height: 45px;
        font-size: 12px;
    }

    .row {
        line-height: 60px;
    }

    .row+.row {
        border-top: 1px solid #d2d2d2;
    }
}
</style>