import { createRouter, createWebHistory, createWebHashHistory, type RouteRecordRaw } from 'vue-router'
import message from '@/utils/message'
import useUserStore from '@/stores/useUserStore'
import useAppStore from '@/stores/useAppStore'

const routeList: RouteRecordRaw[] = [
  {
    path: '/',
    redirect: '/index'
  },
  {
    path: '/index',
    component: () => import('@/views/HomeView.vue'),
  },
  {
    path: '/user',
    component: () => import('@/views/user/IndexView.vue')
  },
  {
    path: '/login',
    component: () => import('@/views/LoginView.vue')
  },
  {
    path: '/work',
    component: () => import('@/views/work/IndexView.vue')
  },
  {
    path: '/card',
    component: () => import('@/views/card/IndexView.vue')
  },
  {
    path: '/order',
    component: () => import('@/views/order/IndexView.vue')
  },
  {
    path: '/404',
    component: () => import('@/views/error/404.vue')
  },
  {
    path: '/flow',
    redirect: '/flow/text',
    component: () => import('@/views/flow/IndexView.vue'),
    children: [
      {
        path: '/flow/prod',
        component: () => import('@/views/flow/form/ProdForm.vue')
      },
      {
        path: '/flow/pic_bg',
        component: () => import('@/views/flow/form/PicForm.vue')
      },
      {
        path: '/flow/upscale',
        component: () => import('@/views/flow/form/UpscaleForm.vue')
      },
      {
        path: '/flow/pic_s',
        component: () => import('@/views/flow/form/SimplePicForm.vue')
      },
      {
        path: '/flow/pic_ext',
        component: () => import('@/views/flow/form/PicExtForm.vue')
      },
      {
        path: '/flow/text',
        component: () => import('@/views/flow/form/TextForm.vue')
      },
      {
        path: '/flow/picp',
        component: () => import('@/views/flow/form/PicPicForm.vue')
      }
    ]
  },
  {
    path: '/:pathMatch(.*)*', // 匹配所有路径
    name: 'NotFound',
    redirect: '/404'
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  //history: createWebHashHistory(import.meta.env.BASE_URL),
  routes: routeList
})

const whiteList = ['/login', '/', '/index', '/404']
router.beforeEach((to, from, next) => {
  //加载
  message.load('路由加载中')
  const appStore = useAppStore()
  if (appStore.flowList == null || appStore.flowList.length == 0) {
    appStore.loadFlow().then(() => {
      // let formRoutes = generateRouet(appStore.flowList)
      // console.log('加载动态路由')
      // console.log(formRoutes)

      // router.addRoute(formRoutes)
      // console.log('动态路由加载完成')
      // console.log(router.getRoutes())
      checkRoute(to, next, 'done')
    }).finally(() => {
      message.close()
    })
  } else {
    checkRoute(to, next, null)
  }
  //checkRoute(to, next)
})

function checkRoute(to, next, state) {
  const userStore = useUserStore()
  let token = userStore.token


  if (whiteList.indexOf(to.path) !== -1) {
    // hack方法 确保addRoutes已完成
    if (state == 'done') {
      next({ ...to, replace: true })
    } else {
      next()
    }
  } else {
    if (!token) {
      //未登录
      userStore.logout()
      next(`/login?redirect=${to.fullPath}`)
    } else {
      // hack方法 确保addRoutes已完成
      if (state == 'done') {
        next({ ...to, replace: true })
      } else {
        next()
      }
    }
  }
}


function generateRouet(flowList: any) {
  let _route = []
  for (let i = 0; i < flowList.length; i++) {
    let flow = flowList[i]
    var path = `@/views/flow/form/${flow.component}`
    // _route.push({
    //   path: flow.route,
    //   component: () => import(path)
    // })
  }
  return {
    path: '/flow',
    redirect: '/flow/text',
    component: () => import('@/views/flow/IndexView.vue'),
    children: _route
  }
}

router.afterEach(() => {
  message.close()
})




export default router
