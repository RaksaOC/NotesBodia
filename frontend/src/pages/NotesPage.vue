<template>
    <div class="notes-page flex w-full">
    <ErrorBanner
      v-if="errorMessage"
      :message="errorMessage"
      :showRetry="true"
      @close="errorMessage = ''"
      @retry="onRetry"
    />
        <SideBar :notes="notes" :selectedNoteId="selectedNoteId" :search="search" :filter="filter"
            :isLoading="isNotesLoading" @onSearch="onSearchChange" @onApplyFilter="onFilterChange"
            @onNoteItemClick="onNoteItemClick" @onCreateNote="onCreateNote" @onDeleteNote="onDeleteNote" />

        <NoteView :note="selectedNote" :isLoading="isNoteLoading" :isSaving="isSavingNote" @onSaveContent="onSaveNote"
            @onSaveTitle="onSaveNote" />
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import SideBar from '../components/pages/notes/SideBar.vue';
import NoteView from '../components/pages/notes/NoteView.vue';
import { notesService } from '@/services/notes.service';
import type { Note, NoteCreateDTO, NoteUpdateDTO } from '@/types/notes';
import { FilterOptions } from '@/constants/filterOptions';
import ErrorBanner from '@/components/common/Error.vue';

const notes = ref<Note[]>([]);
const selectedNoteId = ref<number | null>(null);
const selectedNote = ref<Note | null>(null);

const isNotesLoading = ref(false);
const isNoteLoading = ref(false);
const isSavingNote = ref(false);
const errorMessage = ref('');

const search = ref('');
const filter = ref<FilterOptions>(FilterOptions.LATEST);

const fetchNotes = async () => {
    try {
        isNotesLoading.value = true;
    errorMessage.value = '';
        const response = await notesService.getNotes(search.value, filter.value);
        if (response.success) {
            notes.value = response.data;
        } else {
            throw new Error(response.message);
        }
    } catch (error) {
    console.error(error);
    errorMessage.value = 'Failed to load notes.';
        notes.value = [];
    } finally {
        isNotesLoading.value = false;
    }
};

const fetchNoteById = async (id: number | null) => {
    if (id == null) {
        selectedNote.value = null;
        return;
    }

    try {
        isNoteLoading.value = true;
    errorMessage.value = '';
        const response = await notesService.getNoteById(id);
        if (response.success) {
            selectedNote.value = response.data;
        } else {
            throw new Error(response.message);
        }
    } catch (error) {
    console.error(error);
    errorMessage.value = 'Failed to load note.';
        selectedNote.value = null;
    } finally {
        isNoteLoading.value = false;
    }
};

const onNoteItemClick = async (id: number) => {
    selectedNoteId.value = id;
    await fetchNoteById(id);
};

const onCreateNote = async (title: string) => {
    const payload: NoteCreateDTO = { title, content: '' };
    try {
    errorMessage.value = '';
        const created = await notesService.createNote(payload);
        if (created.success) {
            notes.value.unshift(created.data);
            selectedNoteId.value = created.data.id;
            selectedNote.value = created.data;
        } else {
            throw new Error(created.message);
        }
    } catch (error) {
    console.error(error);
    errorMessage.value = 'Failed to create note.';
    }
};

const onDeleteNote = async (id: number) => {
    try {
    errorMessage.value = '';
        const response = await notesService.deleteNote(id);
        if (response.success) {
            notes.value = notes.value.filter((n) => n.id !== id);
            if (selectedNoteId.value === id) {
                selectedNoteId.value = null;
                selectedNote.value = null;
            }
        } else {
            throw new Error(response.message);
        }
    } catch (error) {
    console.error(error);
    errorMessage.value = 'Failed to delete note.';
    }
};

const onSaveNote = async (update: NoteUpdateDTO) => {
    try {
        isSavingNote.value = true;
    errorMessage.value = '';
        const response = await notesService.updateNote(update);
        if (response.success) {
            selectedNote.value = response.data;
            const index = notes.value.findIndex((n) => n.id === response.data.id);
            if (index !== -1) {
                notes.value[index] = response.data;
            }
        } else {
            throw new Error(response.message);
        }
    } catch (error) {
    console.error(error);
    errorMessage.value = 'Failed to save note.';
    } finally {
        isSavingNote.value = false;
    }
};

const onSearchChange = async (value: string) => {
    search.value = value;
    await fetchNotes();
};

const onFilterChange = async (mode: FilterOptions) => {
    filter.value = mode;
    await fetchNotes();
};

const onRetry = async () => {
  await fetchNotes();
  if (selectedNoteId.value) {
    await fetchNoteById(selectedNoteId.value);
  }
};

onMounted(async () => {
    await fetchNotes();
    if (notes.value.length > 0) {
        const firstId = notes.value[0]?.id ?? null;
        if (firstId) {
            selectedNoteId.value = firstId;
            await fetchNoteById(firstId);
        }
    }
});
</script>