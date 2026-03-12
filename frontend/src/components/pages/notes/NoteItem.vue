// is a chip, where it contain the note title and the created at and short snippet of the content, have a vert dot to
delete

<template>
    <div class="note-item relative  py-2 px-2 hover:bg-zinc-900/50 cursor-pointer"
        :class="{ 'bg-zinc-900 border-b border-pink-800': isSelected }" @click="emit('onNoteItemClick', note.id)">
        <NoteDeletionConfirm v-if="isDeleteModalOpen" :title="note.title" @confirm="confirmDelete"
            @cancel="isDeleteModalOpen = false" />

        <div class="note-item-header flex items-center justify-between">
            <div class="flex flex-col space-y-2">
                <h1 class="text-sm font-medium truncate">{{ note.title }}</h1>
                <div class="text-xs text-zinc-400 line-clamp-1">
                    <p>{{ note.content }}</p>
                </div>
            </div>
            <button @click.stop="isOptionsOpen = !isOptionsOpen">
                <MoreVertical class="w-4 h-4 text-accent" />
            </button>
            <div v-show="isOptionsOpen"
                class="absolute right-2 top-8 mt-2 w-40 bg-zinc-950 border border-zinc-800 shadow-lg text-xs z-20">
                <button type="button"
                    class="w-full text-left px-3 py-2 hover:bg-zinc-900 flex items-center justify-between text-red-400"
                    @click.stop="onDeleteClick">
                    <span>Delete</span>
                </button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import { MoreVertical } from 'lucide-vue-next';

import type { Note } from '@/types/notes';
import NoteDeletionConfirm from './NoteDeletionConfirm.vue';

const props = defineProps<{
    note: Note;
    isSelected: boolean;
}>();

const emit = defineEmits<{
    (e: 'onNoteItemClick', id: number): void;
    (e: 'onDeleteNote', id: number): void;
}>();

const note = computed(() => props.note);
const isOptionsOpen = ref(false);
const isDeleteModalOpen = ref(false);

const onDeleteClick = () => {
    isOptionsOpen.value = false;
    isDeleteModalOpen.value = true;
};

const confirmDelete = () => {
    emit('onDeleteNote', note.value.id);
    isDeleteModalOpen.value = false;
};
</script>