const env = {
	environment: 'development',
	development: {
		apiURL: 'http://localhost:15111',
		serverURL: 'http://localhost:15111',
		fileURL: 'https://www***.com',
		fileViewURL: 'http://****/preview',
		webSocketURL: 'http://localhost:15111/hub'
	},
	production: {
		apiURL: '?',
		serverURL: '?',
		fileURL: '?',
		fileViewURL: '?',
		webSocketURL: '?'
	}
}

const config = () => { 
	if (process.env.NODE_ENV === 'development') {
		return env.development
	} else {
		return env.production
	}
}

const _config = config()
export default _config