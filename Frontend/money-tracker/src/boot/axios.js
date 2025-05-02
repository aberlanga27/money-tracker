import axios from 'axios';

const baseUrl = 'http://localhost:5001/api/v1/';
const api = axios.create( { baseURL: baseUrl } );

export { api };