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
                activities: []
            };
        }
    }

    hanses = [];
    loadHanses() {
        return api.get('hans')
            .then(res => {
                console.log('Loaded hanses', res.data);
                this.hanses = res.data;
                return res.data;
            })
            .catch(e => {
                console.error('error loading hanses', e);
            });
    }
    hansAt(x: number, y: number) {
        return this.hanses.filter(h => h.location.coordinates.x === x && h.location.coordinates.y === y)[0];
    }

    tick() {
        return api.post('tick');
    }

    queueActivity(hansId: number, locationId: number, activity: string) {
        return api.post(`hans/${hansId}/do`, {
            activity,
            location: locationId
        });

    }

}

export const Backend = new BackendImpl();
