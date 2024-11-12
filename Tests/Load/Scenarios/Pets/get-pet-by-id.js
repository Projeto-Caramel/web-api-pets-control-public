import http from 'k6/http';
import { check } from 'k6';
import { Trend, Rate, Counter } from 'k6/metrics';

let getPetDuration = new Trend('get_pet_duration');
let getPetFailRate = new Rate('get_pet_fail_rate');
let getPetSuccessRate = new Rate('get_pet_success_rate');
let getPetRequests = new Counter('get_pet_requests');

export default function () {
    const url = 'https://localhost:7127/pets-control/pets/6688a0658b03ac6f55eed71f';
    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    let response = http.get(url, params);

    console.log('GET Pet by ID Response status: ', response.status);
    console.log('GET Pet by ID Response body: ', response.body);

    getPetDuration.add(response.timings.duration);

    if (response.status >= 200 && response.status < 400) {
        getPetSuccessRate.add(1);
    } else {
        getPetFailRate.add(1);
    }

    getPetRequests.add(1);

    check(response, {
        'status is 200 (getPetById)': (r) => r.status === 200,
        'response time is less than 4000ms (getPetById)': (r) => r.timings.duration < 4000,
    });
}
