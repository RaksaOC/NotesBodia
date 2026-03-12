<template>
  <div class="animate-pulse space-y-2 p-2">
    <div v-for="i in count" :key="i" class="flex items-center justify-between gap-2 py-2 px-2 ">
      <div class="flex flex-col gap-2 min-w-0 flex-1">
        <div class="h-4  bg-zinc-800 w-3/4" />
        <div class="h-3  bg-zinc-800/80 w-full max-w-[90%]" />
      </div>
      <div class="h-4 w-4 shrink-0  bg-zinc-800/80" />
    </div>
  </div>
</template>

<script setup lang="ts">
withDefaults(
  defineProps<{
    count?: number;
  }>(),
  { count: 5 }
);
</script>
