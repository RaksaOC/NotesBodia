// a modal to input note title when creating or editing

<template>
    <div class="note-title-modal fixed inset-0 bg-black/10 backdrop-blur-sm z-10 flex justify-center items-center">
        <div class="bg-background backdrop-blur-xl border border-zinc-800 rounded-lg p-4 w-full max-w-md card-contrast space-y-3">
            <h1 class="text-sm font-semibold tracking-[0.18em] uppercase text-zinc-300">
                Note title
            </h1>
            <input type="text" placeholder="Enter title" class="input-sharp w-full" v-model="title"
                @keyup.enter.prevent="onSave" />
            <div class="flex justify-end gap-2">
                <button class="btn-secondary px-4 py-1 text-sm" @click="onClose">
                    Close
                </button>
                <button class="btn-primary px-4 py-1 text-sm" @click="onSave">
                    Save
                </button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';

const props = defineProps<{
    isOpen: boolean;
    title: string;
}>();

const emit = defineEmits<{
    (e: 'onSave', title: string): void;
    (e: 'onClose'): void;
}>();

const title = ref(props.title);

watch(
    () => props.title,
    (newTitle) => {
        title.value = newTitle;
    },
    { immediate: true },
);

const onSave = () => {
    emit('onSave', title.value.trim());
};

const onClose = () => {
    emit('onClose');
};
</script>