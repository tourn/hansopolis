import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5000/api/game'
});

class BackendImpl {
    locations = [];
    loadLocations(): Promise<any> {
        return api.get('location')
            .then(res => {
                console.log('Loaded map', res.data);
                this.locations = res.data;
                return res.data;
            })
            .catch(e => {
                console.error('error loading map', e);
            });

    }
    locationAt(x: number, y: number) {
        const l = this.locations.filter(h => h.coordinates.x === x && h.coordinates.y === y)[0];
        if (l) {
            return l;
        } else {
            return {
                name: 'Nothing',
                actions: []
            };
        }
    }

    queueActivity(hansId: number, locationId: number, activity: string) {
        return api.post(`hans/${hansId}/do`, {
            activity,
            location: locationId
        });

    }

}

export const Backend = new BackendImpl();
