<template>
  <form class="space-y-4" @submit.prevent="onSubmit">
    <div class="space-y-2">
      <label class="text-xs font-medium tracking-wide uppercase text-zinc-300">
        Email
      </label>
      <input
        class="input-sharp"
        type="email"
        placeholder="you@example.com"
        autocomplete="email"
        v-model.trim="email"
      />
      <p v-if="emailError" class="text-xs text-red-500">
        {{ emailError }}
      </p>
    </div>

    <div class="space-y-2">
      <label class="text-xs font-medium tracking-wide uppercase text-zinc-300">
        Password
      </label>
      <input
        class="input-sharp"
        type="password"
        placeholder="••••••••"
        autocomplete="new-password"
        v-model.trim="password"
      />
      <p v-if="passwordError" class="text-xs text-red-500">
        {{ passwordError }}
      </p>
    </div>

    <div class="space-y-2">
      <label class="text-xs font-medium tracking-wide uppercase text-zinc-300">
        Confirm password
      </label>
      <input
        class="input-sharp"
        type="password"
        placeholder="••••••••"
        autocomplete="new-password"
        v-model.trim="confirmPassword"
      />
      <p v-if="confirmPasswordError" class="text-xs text-red-500">
        {{ confirmPasswordError }}
      </p>
    </div>

    <button
      type="submit"
      class="btn-primary w-full mt-2 flex items-center justify-center gap-2"
      :disabled="props.isLoading"
    >
      <div
        v-if="props.isLoading"
        class="animate-spin rounded-full h-5 w-5 border-b-2 border-white"
      ></div>
      <p v-else>Create account</p>
    </button>
  </form>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import type { SignupDTO } from '@/types/auth';

const emit = defineEmits<{
  (e: 'onSubmit', signupDTO: SignupDTO): void;
}>();

const props = defineProps<{
  isLoading: boolean;
}>();

const email = ref<string>('');
const password = ref<string>('');
const confirmPassword = ref<string>('');

const emailError = ref<string>('');
const passwordError = ref<string>('');
const confirmPasswordError = ref<string>('');

const validate = () => {
  emailError.value = '';
  passwordError.value = '';
  confirmPasswordError.value = '';

  if (!email.value) {
    emailError.value = 'Email is required';
  } else if (!/^\S+@\S+\.\S+$/.test(email.value)) {
    emailError.value = 'Enter a valid email address';
  }

  if (!password.value) {
    passwordError.value = 'Password is required';
  } else if (password.value.length < 6) {
    passwordError.value = 'Password must be at least 6 characters';
  }

  if (!confirmPassword.value) {
    confirmPasswordError.value = 'Please confirm your password';
  } else if (password.value !== confirmPassword.value) {
    confirmPasswordError.value = 'Passwords do not match';
  }

  return !emailError.value && !passwordError.value && !confirmPasswordError.value;
};

const onSubmit = () => {
  if (!validate()) return;

  const signupDTO: SignupDTO = {
    email: email.value,
    password: password.value,
  };

  emit('onSubmit', signupDTO);
};
</script>

