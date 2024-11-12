import getPets from "./Scenarios/Pets/get-pets.js";
import getPetById from "./Scenarios/Pets/get-pet-by-id.js";
import getPetStatus from "./Scenarios/Pets/get-pet-status.js";
import { group, sleep } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/2.2.0/dist/bundle.js";
import { textSummary } from "https://jslib.k6.io/k6-summary/0.0.1/index.js";

export let options = {
    stages: [
        { duration: '30s', target: 10 }, // Ramp-up para 10 usuários durante 30 segundos
        { duration: '1m', target: 10 },  // Mantém 10 usuários por 1 minuto
        { duration: '30s', target: 0 },  // Ramp-down para 0 usuários durante 30 segundos
    ],
    thresholds: {
        'get_pets_duration': ['p(95)<4000'], // 95% das requisições devem ser completadas em menos de 4000ms
        'get_pets_fail_rate': ['rate<0.05'], // Taxa de falhas deve ser menor que 5%
        'get_pet_duration': ['p(95)<4000'],
        'get_pet_fail_rate': ['rate<0.05'],
        'get_pet_status_duration': ['p(95)<4000'],
        'get_pet_status_fail_rate': ['rate<0.05'],
    },
};

export default function () {
    group('Load Test for Get Pets Endpoint', () => {
        getPets();
    });

    group('Load Test for Get Pet by ID Endpoint', () => {
        getPetById();
    });

    group('Load Test for Get Pet Status Endpoint', () => {
        getPetStatus();
    });

    sleep(1);
}

// Função para gerar o relatório HTML e JSON
export function handleSummary(data) {
    return {
        "summary.html": htmlReport(data),
        "summary.json": JSON.stringify(data, null, 2),
        stdout: textSummary(data, { indent: " ", enableColors: true }),
    };
}
