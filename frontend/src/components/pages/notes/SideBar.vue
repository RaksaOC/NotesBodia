// will contain a header at the top (sth like My Notes and to the right is a + button) and below is the search and
// filter and then below that is the list of note chips

<template>
    <div>
        <NoteTitleModal v-if="isCreateModalOpen" :title="newNoteTitle" :isOpen="isCreateModalOpen"
            @onSave="onCreateSave" @onClose="isCreateModalOpen = false" />

        <!-- Mobile top bar -->
        <div
            class="lg:hidden fixed top-0 inset-x-0 h-14 px-4 flex items-center justify-between bg-background border-b border-zinc-800 z-30">
            <h1 class="text-lg font-semibold tracking-[0.1em] uppercase">
                NOTESBODIA
            </h1>
            <button type="button" class="btn-primary px-2 py-1 flex items-center justify-center"
                @click="isMobileNavOpen = !isMobileNavOpen">
                <Menu class="w-4 h-4 text-foreground" />
            </button>
        </div>

        <!-- Mobile slide-over sidebar -->
        <div v-if="isMobileNavOpen" class="lg:hidden fixed inset-0 z-40 flex">
            <div class="flex-1 bg-black/40" @click="isMobileNavOpen = false"></div>
            <div
                class="w-64 max-w-[80%] h-full bg-background text-foreground border-l border-zinc-800 flex flex-col relative">
                <div class="sidebar-header flex items-center justify-between px-3 py-4 border-b border-zinc-800">
                    <h1 class="text-lg font-semibold tracking-[0.18em] uppercase">
                        NOTESBODIA
                    </h1>
                    <button class="btn-primary p-2" @click="isMobileNavOpen = false">
                        <X class="w-4 h-4 text-foreground" />
                    </button>
                </div>

                <div class="flex items-center justify-between py-4">
                    <SearchFilter :search="search" :filter="filter" @onSearch="(val: string) => emit('onSearch', val)"
                        @onApplyFilter="(val: FilterOptions) => emit('onApplyFilter', val)" />

                    <button class="btn-primary mr-3" @click="onOpenCreateNoteModal">
                        <Plus class="w-4 h-4 text-foreground" />
                    </button>
                </div>

                <div class="sidebar-content flex-1 overflow-y-auto scrollbar-hide pb-20">
                    <SidebarSkeleton v-if="isLoading" :count="5" />
                    <div v-else class="p-2 space-y-2">
                        <NoteItem v-for="note in notes" :key="note.id" :note="note"
                            :isSelected="note.id === selectedNoteId" @onNoteItemClick="onNoteItemClick"
                            @onDeleteNote="emit('onDeleteNote', note.id)" />
                    </div>
                </div>

                <div
                    class="bg-background absolute bottom-0 left-0 right-0 border-t border-zinc-800 flex justify-between items-center px-3 py-4">
                    <p class="text-sm text-zinc-400">{{ email }}</p>
                    <button class="btn-primary" @click="logout">
                        <LogOut class="w-4 h-4 text-foreground" />
                    </button>
                </div>
            </div>
        </div>

        <!-- Desktop sidebar -->
        <div
            class="sidebar h-[calc(100vh-3rem)] hidden lg:fixed left-6 top-6 w-68 lg:flex lg:flex-col bg-background text-foreground border border-zinc-800">
            <div class="sidebar-header flex items-center justify-between px-2 py-4 border-b border-zinc-800">
                <h1 class="text-xl font-semibold tracking-[0.1em] uppercase">
                    NOTESBODIA
                </h1>
                <button class="btn-primary p-2" @click="onOpenCreateNoteModal">
                    <Plus class="w-4 h-4 text-foreground" />
                </button>
            </div>

            <SearchFilter :search="search" :filter="filter" @onSearch="(val: string) => emit('onSearch', val)"
                @onApplyFilter="(val: FilterOptions) => emit('onApplyFilter', val)" />

            <div class="sidebar-content flex-1 overflow-y-auto scrollbar-hide">
                <SidebarSkeleton v-if="isLoading" :count="5" />
                <div v-else class="p-2 space-y-2">
                    <NoteItem v-for="note in notes" :key="note.id" :note="note" :isSelected="note.id === selectedNoteId"
                        @onNoteItemClick="onNoteItemClick" @onDeleteNote="emit('onDeleteNote', note.id)" />
                </div>
            </div>

            <div
                class="bg-background absolute bottom-0 left-0 right-0 border-t border-zinc-800 flex justify-between items-center px-2 py-4">
                <p class="text-sm text-zinc-400">{{ email }}</p>
                <button class="btn-primary" @click="logout">
                    <LogOut class="w-4 h-4 text-foreground" />
                </button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import { Plus, LogOut, Menu, X } from 'lucide-vue-next';
import NoteItem from './NoteItem.vue';
import SidebarSkeleton from './SidebarSkeleton.vue';
import SearchFilter from './SearchFilter.vue';
import NoteTitleModal from './NoteTitleModal.vue';
import type { Note } from '@/types/notes';
import { authService } from '@/services/auth.service';
import { useRouter } from 'vue-router';
import { FilterOptions } from '@/constants/filterOptions';

const emit = defineEmits<{
    (e: 'onNoteItemClick', id: number): void;
    (e: 'onCreateNote', title: string): void;
    (e: 'onDeleteNote', id: number): void;
    (e: 'onSearch', value: string): void;
    (e: 'onApplyFilter', value: FilterOptions): void;
}>();

const props = defineProps<{
    notes: Note[];
    selectedNoteId: number | null;
    search: string;
    filter: FilterOptions;
    isLoading: boolean;
}>();

const router = useRouter();

const isMobileNavOpen = ref(false);
const isCreateModalOpen = ref(false);
const newNoteTitle = ref('');

const logout = async () => {
    await authService.logout();
    router.push('/auth');
};

const onOpenCreateNoteModal = () => {
    newNoteTitle.value = '';
    isCreateModalOpen.value = true;
};

const onCreateSave = (title: string) => {
    if (!title) {
        isCreateModalOpen.value = false;
        return;
    }
    emit('onCreateNote', title);
    isCreateModalOpen.value = false;
};

const onNoteItemClick = (id: number) => {
    if (id === props.selectedNoteId) {
        isMobileNavOpen.value = false;
        return;
    }
    isMobileNavOpen.value = false;
    emit('onNoteItemClick', id);
};

const search = computed(() => props.search);
const filter = computed(() => props.filter);
const notes = computed(() => props.notes);
const email = computed(() => localStorage.getItem('email'));
</script>
