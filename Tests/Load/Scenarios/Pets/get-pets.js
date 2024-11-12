import http from 'k6/http';
import { check } from 'k6';
import { Trend, Rate, Counter } from 'k6/metrics';

let getPetsDuration = new Trend('get_pets_duration');
let getPetsFailRate = new Rate('get_pets_fail_rate');
let getPetsSuccessRate = new Rate('get_pets_success_rate');
let getPetsRequests = new Counter('get_pets_requests');

export default function () {
    const url = 'https://localhost:7127/pets-control/pets';
    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    const partnerId = '666caa1bee2bbd83699dd5d5';

    let response = http.get(`${url}?partnerId=${partnerId}&page=1&size=10`, params);

    console.log('GET Pets Response status: ', response.status);
    console.log('GET Pets Response body: ', response.body);

    getPetsDuration.add(response.timings.duration);

    if (response.status >= 200 && response.status < 400) {
        getPetsSuccessRate.add(1);
    } else {
        getPetsFailRate.add(1);
    }

    getPetsRequests.add(1);

    check(response, {
        'status is 200 (getPets)': (r) => r.status === 200,
        'response time is less than 4000ms (getPets)': (r) => r.timings.duration < 4000,
    });
}
