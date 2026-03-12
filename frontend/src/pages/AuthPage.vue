<template>
  <main class="min-h-screen bg-background text-foreground flex items-center justify-center px-6">
    <ErrorBanner v-if="errorMessage" :message="errorMessage" :showRetry="false" @close="errorMessage = ''" />
    <div class="w-full max-w-md space-y-6 card-contrast p-8">
      <AuthTabs :activeTab="activeTab" @onTabClick="onTabClick" />
      <LogIn v-if="activeTab === AuthTabsEnum.LOGIN" @onSubmit="onLoginSubmit" :isLoading="isLoading" />
      <SignUp v-if="activeTab === AuthTabsEnum.SIGNUP" @onSubmit="onSignUpSubmit" :isLoading="isLoading" />
    </div>
  </main>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { AuthTabs as AuthTabsEnum } from '@/constants/authTabs'
import AuthTabs from '@/components/pages/auth/AuthTabs.vue'
import LogIn from '@/components/pages/auth/LogIn.vue'
import SignUp from '@/components/pages/auth/SignUp.vue'
import type { LoginDTO, SignupDTO } from '@/types/auth'
import { authService } from '@/services/auth.service'
import { useRouter } from 'vue-router'
import { routesMap } from '@/router/router'
import ErrorBanner from '@/components/common/Error.vue'

const activeTab = ref<AuthTabsEnum>(AuthTabsEnum.LOGIN)

const router = useRouter()


const errorMessage = ref<string>('')
const isLoading = ref<boolean>(false)

const onTabClick = (tab: AuthTabsEnum) => {
  activeTab.value = tab
}

const onLoginSubmit = async (loginDTO: LoginDTO) => {
  try {
    isLoading.value = true
    errorMessage.value = ''
    const response = await authService.login(loginDTO)
    if (response.success) {
      localStorage.setItem('token', response.data.token)
      localStorage.setItem('email', response.data.email)
      router.push(routesMap.NOTES)
    } else {
      errorMessage.value = response.message
    }
  } catch (error) {
    errorMessage.value = 'Something went wrong while logging in.'
  } finally {
    isLoading.value = false
  }
}

const onSignUpSubmit = async (signUpDTO: SignupDTO) => {
  try {
    isLoading.value = true
    errorMessage.value = ''
    const response = await authService.signup(signUpDTO)
    if (response.success) {
      localStorage.setItem('token', response.data.token)
      localStorage.setItem('email', response.data.email)
      router.push(routesMap.NOTES)
    } else {
      errorMessage.value = response.message
    }
  } catch (error) {
    errorMessage.value = 'Something went wrong while signing up.'
  } finally {
    isLoading.value = false
  }
}
</script>
