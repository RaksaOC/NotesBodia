export interface Note {
    id: number;
    title: string;
    content?: string;
    createdAt: string;
    updatedAt: string;
}

export interface NoteCreateDTO {
    title: string;
    content: string;
}

// either update a title or content never both, backend check these optionals
export interface NoteUpdateDTO {
    id: number;
    title?: string;
    content?: string;
}
