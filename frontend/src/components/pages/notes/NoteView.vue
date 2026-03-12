// will be the canvas to edit the note, should notify if sth change so that the save button is clickable, and also
should look seem less

<template>
    <NoteTitleModal v-if="isEditTitleModalOpen" :title="editableTitle" :isOpen="isEditTitleModalOpen"
        @onSave="onSaveTitle" @onClose="isEditTitleModalOpen = false" />
    <NotesSkeleton v-if="isLoading" />
    <NoteNotSelected v-else-if="!note" />
    <section v-else class="mt-20 mx-4 bg-background border border-zinc-800 h-[calc(100vh-6rem)] w-full space-y-2
           lg:mt-6 lg:ml-80 lg:mr-6 lg:h-[calc(100vh-3rem)]">
        <div class="flex justify-between items-center px-4 py-2.5">
            <div class="flex items-center space-x-2">
                <h1 class="text-lg lg:text-2xl font-semibold truncate">{{ editableTitle }}</h1>
                <button @click="isEditTitleModalOpen = true" class="btn-secondary p-1 hover:bg-zinc-900/50">
                    <Pencil class="w-4 h-4 text-accent" />
                </button>
            </div>
            <button class="btn-primary flex items-center gap-2" :disabled="isSaving" @click="onSaveContent">
                <span v-if="isSaving" class="animate-spin rounded-full h-4 w-4 border-b-2 border-white" />
                <p>Save</p>
            </button>
        </div>
        <div class="h-px bg-zinc-800 my-2"></div>

        <div class="p-2">
            <textarea
                class="input-soft w-full h-full resize-none lg:min-h-[calc(100vh-10rem)] min-h-[calc(100vh-12rem)] focus:outline-none focus:ring-0 focus:border-none !border-none"
                placeholder="My Note Content" v-model="editableContent" />
        </div>
    </section>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import type { Note, NoteUpdateDTO } from '@/types/notes';
import NotesSkeleton from './NotesSkeleton.vue';
import NoteNotSelected from './NoteNotSelected.vue';
import NoteTitleModal from './NoteTitleModal.vue';
import { Pencil } from 'lucide-vue-next';

const props = defineProps<{
    note: Note | null;
    isLoading: boolean;
    isSaving: boolean;
}>();

const emit = defineEmits<{
    (e: 'onSaveContent', update: NoteUpdateDTO): void;
    (e: 'onSaveTitle', update: NoteUpdateDTO): void;
}>();

const editableTitle = ref('');
const editableContent = ref<string | undefined>('');
const isEditTitleModalOpen = ref(false);

watch(
    () => props.note,
    (newNote) => {
        editableTitle.value = newNote?.title ?? '';
        editableContent.value = newNote?.content ?? '';
        isEditTitleModalOpen.value = false;
    },
    { immediate: true },
);

const note = computed(() => props.note);
const isLoading = computed(() => props.isLoading);
const isSaving = computed(() => props.isSaving);

const onSaveTitle = (title: string) => {
    if (!note.value) return;

    const update: NoteUpdateDTO = {
        id: note.value.id,
        title: title,
    };
    // update ui but call to the same onSave handler at parent
    editableTitle.value = title;
    isEditTitleModalOpen.value = false;
    emit('onSaveTitle', update);

};

const onSaveContent = () => {
    if (!note.value) return;

    const update: NoteUpdateDTO = {
        id: note.value.id,
        content: editableContent.value,
    };
    emit('onSaveContent', update);


};
</script>
