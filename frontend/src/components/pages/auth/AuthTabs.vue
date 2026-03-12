<template>
    <div class="w-full max-w-md   space-y-6">
        <header class="flex flex-col justify-center items-center space-y-1 pb-4">
            <p class="text-xs font-semibold text-pink-500 uppercase tracking-[0.1em]">
                NOTESBODIA
            </p>
            <h1 class="text-2xl font-semibold">
                {{ activeTab === 'login' ? 'Welcome back' : 'Create your account' }}
            </h1>
        </header>

        <nav class="flex gap-2 text-md uppercase ">
            <button type="button" class="flex-1 py-2 border-b-2"
                :class="activeTab === 'login' ? 'border-pink-500 text-zinc-50' : 'border-zinc-800 text-zinc-500'"
                @click="onTabClick(AuthTabs.LOGIN)">
                Log in
            </button>
            <button type="button" class="flex-1 py-2 border-b-2"
                :class="activeTab === 'signup' ? 'border-pink-500 text-zinc-50' : 'border-zinc-800 text-zinc-500'"
                @click="onTabClick(AuthTabs.SIGNUP)">
                Sign up
            </button>
        </nav>

    </div>
</template>

<script setup lang="ts">
import { AuthTabs } from '@/constants/authTabs';

const props = defineProps<{
    activeTab: AuthTabs
}>()

const emit = defineEmits<{
    (e: 'onTabClick', tab: AuthTabs): void
}>()

const onTabClick = (tab: AuthTabs) => {
    emit('onTabClick', tab)
}

</script>
