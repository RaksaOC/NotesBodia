import { createRouter, createWebHashHistory, createWebHistory } from 'vue-router';
import AuthPage from '@/pages/AuthPage.vue';
import NotesPage from '@/pages/NotesPage.vue';
import NotFoundPage from '@/pages/NotFoundPage.vue';

export const routesMap = {
  AUTH: "/auth",
  NOTES: "/notes",
}

const routes = [
  {
    path: '/',
    redirect: routesMap.AUTH,
  },
  {
    path: routesMap.AUTH,
    name: 'auth',
    component: AuthPage,
  },
  {
    path: routesMap.NOTES,
    name: 'notes',
    component: NotesPage,
    meta: { requiresAuth: true },
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'not-found',
    component: NotFoundPage,
  },
];

export const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')

  if (to.matched.some(record => record.meta.requiresAuth) && !token) {
    next(routesMap.AUTH)
    return
  }

  if (to.path === routesMap.AUTH && token) {
    next(routesMap.NOTES)
    return
  }

  next()
})

