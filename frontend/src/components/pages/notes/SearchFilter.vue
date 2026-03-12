<template>
    <div class="search-filter relative px-2 py-2">
        <div class="relative">
            <input class="input-sharp pr-9" type="text" :value="search" placeholder="Search notes..."
                @input="onInput" />

            <button type="button"
                class="absolute inset-y-0 right-1 my-1 px-2 flex items-center justify-center border border-zinc-700 bg-zinc-900 hover:bg-zinc-800 text-xs text-zinc-200 gap-1"
                @click="isOpen = !isOpen">
                <Filter class="w-3 h-3" />
            </button>
        </div>

        <div v-if="isOpen" class="absolute right-2 mt-2 w-40 bg-zinc-950 border border-zinc-800 shadow-lg text-xs z-20">
            <button v-for="option in options" :key="option.value" type="button"
                class="w-full text-left px-3 py-2 hover:bg-zinc-900 flex items-center justify-between"
                :class="{ 'bg-zinc-900': selectedOption === option.value }" @click="select(option.value)">
                <span>{{ option.label }}</span>
                <span v-if="selectedOption === option.value" class="text-pink-500">●</span>
            </button>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import { Filter } from 'lucide-vue-next';
import { FilterOptions } from '@/constants/filterOptions';

const props = defineProps<{
    search: string;
    filter: FilterOptions;
}>();

const emit = defineEmits<{
    (e: 'onSearch', value: string): void;
    (e: 'onApplyFilter', value: FilterOptions): void;
}>();

const isOpen = ref(false);
const selectedOption = ref<FilterOptions | null>(null);

const options: { value: FilterOptions; label: string }[] = [
    { value: FilterOptions.LATEST, label: 'Latest first' },
    { value: FilterOptions.OLDEST, label: 'Oldest first' },
    { value: FilterOptions.LAST_ACCESSED, label: 'Last accessed' },
    { value: FilterOptions.ALPHABETICAL, label: 'Alphabetical' },
];

let searchDebounceId: ReturnType<typeof setTimeout> | null = null;

const onInput = (event: Event) => {
    const target = event.target as HTMLInputElement;
    const value = target.value;

    if (searchDebounceId) {
        clearTimeout(searchDebounceId);
    }

    searchDebounceId = setTimeout(() => {
        emit('onSearch', value);
    }, 300);
};

const select = (value: FilterOptions) => {
    selectedOption.value = value;
    emit('onApplyFilter', value);
    setTimeout(() => {
        isOpen.value = false;
    }, 500);

};

const search = computed(() => props.search);
const filter = computed(() => selectedOption.value);
</script>