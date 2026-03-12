<template>
  <main
    class="min-h-screen bg-background text-foreground flex items-center justify-center px-6"
  >
    <section class="card-contrast max-w-md w-full p-8 space-y-4 text-center">
      <h1 class="text-2xl font-semibold tracking-[0.18em] uppercase">
        404 · Not Found
      </h1>
      <p class="text-sm text-zinc-400">
        The page you’re looking for doesn’t exist or has been moved.
      </p>
      <div class="flex justify-center gap-3 pt-2">
        <RouterLink
          to="/notes"
          class="btn-primary px-4 py-2 text-sm"
        >
          Go to Notes
        </RouterLink>
        <RouterLink
          to="/auth"
          class="px-4 py-2 text-sm border border-zinc-700 text-zinc-200 hover:bg-zinc-900"
        >
          Back to Auth
        </RouterLink>
      </div>
    </section>
  </main>
</template>

<script setup lang="ts">
</script>

