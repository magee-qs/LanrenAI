import { request } from "@/utils/request";

export function upload(formData) {
    return request.post('/common/upload', formData,
        {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
}