import http from 'k6/http';
import { check } from 'k6';
import { Trend, Rate, Counter } from 'k6/metrics';

let getPetStatusDuration = new Trend('get_pet_status_duration');
let getPetStatusFailRate = new Rate('get_pet_status_fail_rate');
let getPetStatusSuccessRate = new Rate('get_pet_status_success_rate');
let getPetStatusRequests = new Counter('get_pet_status_requests');

export default function () {
    const url = 'https://localhost:7127/pets-control/pets/6688a0658b03ac6f55eed71f/status';
    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    let response = http.get(url, params);

    console.log('GET Pet Status Response status: ', response.status);
    console.log('GET Pet Status Response body: ', response.body);

    getPetStatusDuration.add(response.timings.duration);

    if (response.status >= 200 && response.status < 400) {
        getPetStatusSuccessRate.add(1);
    } else {
        getPetStatusFailRate.add(1);
    }

    getPetStatusRequests.add(1);

    check(response, {
        'status is 200 (getPetStatus)': (r) => r.status === 200,
        'response time is less than 4000ms (getPetStatus)': (r) => r.timings.duration < 4000,
    });
}
