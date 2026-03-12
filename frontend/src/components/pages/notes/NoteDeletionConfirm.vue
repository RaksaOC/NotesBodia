<template>
  <div class="fixed inset-0 bg-black/40 backdrop-blur-sm z-40 flex items-center justify-center">
    <div class="bg-background border border-zinc-800 card-contrast px-6 py-5 max-w-sm w-[90%] space-y-3">
      <h2 class="text-sm font-semibold tracking-[0.18em] uppercase text-zinc-300">
        Delete note
      </h2>
      <p class="text-sm text-zinc-400">
        Are you sure you want to delete
        <span class="font-medium text-zinc-100">"{{ title }}"</span>? This action cannot be undone.
      </p>
      <div class="flex justify-end gap-2 pt-2">
        <button
          type="button"
          class="px-3 py-1 text-xs font-medium uppercase tracking-wide border border-zinc-700 text-zinc-200 hover:bg-zinc-900"
          @click="$emit('cancel')"
        >
          Cancel
        </button>
        <button
          type="button"
          class="px-3 py-1 text-xs font-medium uppercase tracking-wide bg-red-600 text-white hover:bg-red-500"
          @click="$emit('confirm')"
        >
          Delete
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  title: string;
}>();

defineEmits<{
  (e: 'confirm'): void;
  (e: 'cancel'): void;
}>();
</script>

