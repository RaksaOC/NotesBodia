import api from "@/config/axios";
import type { Note, NoteCreateDTO, NoteUpdateDTO } from "@/types/notes";
import { FilterOptions } from "@/constants/filterOptions";
import type { ApiResponse } from "@/types/api";

export const notesService = {
    getNotes: async (search?: string | null, filter?: FilterOptions | null): Promise<ApiResponse<Note[]>> => {
        let query = '';
        if (search !== undefined && search !== null && search !== '') {
            query += (query ? '&' : '') + `search=${encodeURIComponent(search)}`;
        }
        if (filter !== undefined && filter !== null) {
            query += (query ? '&' : '') + `filter=${encodeURIComponent(filter)}`;
        }
        const response = await api.get<ApiResponse<Note[]>>(`/api/notes${query ? `?${query}` : ''}`);
        return response.data;
    },

    getNoteById: async (id: number): Promise<ApiResponse<Note>> => {
        const response = await api.get<ApiResponse<Note>>(`/api/notes/${id}`);
        return response.data;
    },
    createNote: async (note: NoteCreateDTO): Promise<ApiResponse<Note>> => {
        const response = await api.post<ApiResponse<Note>>('/api/notes', note);
        return response.data;
    },
    updateNote: async (note: NoteUpdateDTO): Promise<ApiResponse<Note>> => {
        const response = await api.put<ApiResponse<Note>>(`/api/notes/${note.id}`, note);
        return response.data;
    },
    deleteNote: async (id: number): Promise<ApiResponse<void>> => {
        const response = await api.delete<ApiResponse<void>>(`/api/notes/${id}`);
        return response.data;
    },
}