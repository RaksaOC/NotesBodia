export interface LoginDTO {
    email: string;
    password: string;
}

export interface SignupDTO {
    email: string;
    password: string;
}

export interface AuthResponse {
    token: string,
    email: string
}