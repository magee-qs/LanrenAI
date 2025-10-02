const sessionCache = {
    set(key: string, value: string) {
        if (!!key && !!value) {
            sessionStorage.setItem(key, value)
        }
    },
    get(key: string) {
        if (!!key) {
            return sessionStorage.getItem(key)
        }
        return null
    },
    remove(key: string) {
        if (!!key) {
            sessionStorage.removeItem(key)
        }
    },
    setJSON(key: string, value: object) {
        if (!!key && !!value) {
            sessionStorage.setItem(key, JSON.stringify(value))
        }
    },
    getJSON(key: string) {
        if (!!key) {
            const str = sessionStorage.getItem(key)
            if (!!str) {
                return JSON.parse(str)
            }
        }
        return null
    }
}

const localCache = {
    set(key: string, value: string) {
        if (!!key && !!value) {
            localStorage.setItem(key, value)
        }
    },
    get(key: string) {
        if (!!key) {
            return localStorage.getItem(key)
        }
        return null
    },
    remove(key: string) {
        if (!!key) {
            localStorage.removeItem(key)
        }
    },
    setJSON(key: string, value: object) {
        if (!!key && !!value) {
            localStorage.setItem(key, JSON.stringify(value))
        }
    },
    getJSON(key: string) {
        if (!!key) {
            const str = localStorage.getItem(key)
            if (!!str) {
                return JSON.parse(str)
            }
        }
        return null
    }
}


export default { session: sessionCache, local: localCache }