<template>
    <div class="fixed inset-x-0 top-4 flex justify-center z-50">
        <div class="max-w-md w-[90%] card-contrast px-4 py-3 space-y-3">
            <div class="flex items-start gap-2">
                <span class="mt-0.5 text-sm font-semibold uppercase tracking-[0.18em] text-red-600">
                    Error
                </span>
            </div>
            <p class="text-sm leading-snug">
                {{ message }}
            </p>
            <div class="flex justify-end gap-2 pt-1">
                <button type="button"
                    class="px-3 py-1 text-xs font-medium uppercase tracking-wide border border-red-700 text-red-100 hover:bg-red-900"
                    @click="$emit('close')">
                    {{ closeLabelComputed }}
                </button>
                <button v-if="showRetryComputed" type="button"
                    class="px-3 py-1 text-xs font-medium uppercase tracking-wide bg-red-600 text-white hover:bg-red-500"
                    @click="$emit('retry')">
                    {{ retryLabelComputed }}
                </button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
    message: string;
    closeLabel?: string;
    retryLabel?: string;
    showRetry?: boolean;
}>();

defineEmits<{
    (e: 'close'): void;
    (e: 'retry'): void;
}>();

const closeLabelComputed = computed(() => props.closeLabel ?? 'Close');
const retryLabelComputed = computed(() => props.retryLabel ?? 'Retry');
const showRetryComputed = computed(() => props.showRetry ?? false);
</script>

// props: message, and an okay button, or retry... based on props
