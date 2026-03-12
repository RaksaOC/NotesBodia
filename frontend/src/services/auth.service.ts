import api from "@/config/axios";
import type { ApiResponse } from "@/types/api";
import type { AuthResponse, LoginDTO, SignupDTO } from "@/types/auth";

export const authService = {
    login: async (loginDTO: LoginDTO): Promise<ApiResponse<AuthResponse>> => {
        const response = await api.post<ApiResponse<AuthResponse>>('/api/auth/login', loginDTO);
        return response.data;
    },
    signup: async (signupDTO: SignupDTO): Promise<ApiResponse<AuthResponse>> => {
        const response = await api.post<ApiResponse<AuthResponse>>('/api/auth/signup', signupDTO);
        return response.data;
    },

    logout: async (): Promise<void> => {
        localStorage.removeItem('token');
        localStorage.removeItem('email');
    }
}