import * as Assets from '../assets';



const rand = myArray => myArray[Math.floor(Math.random() * myArray.length)];
const TILE_HOUSE = 4;
const TILES_EMPTY = [0, 3, 5];
const TILE_HANS = 11;

const SCALE = 1;
const TILE_WIDTH = 16;

const hanses = [
    {
        name: 'Hans',
        face: 'o,O',
        location: {x: 4, y: 4}
    },
    {
        name: 'Peter',
        face: '>.O',
        location: {x: 6, y: 8}
    },
];

const poi = [
    {
        name: 'Restaurant',
        location: {x: 4, y: 5},
        tile: TILE_HOUSE
    },
    {
        name: 'Home',
        location: {x: 9, y: 9},
        tile: TILE_HOUSE
    },
    {
        name: 'Forest',
        location: {x: 2, y: 7},
        tile: TILE_HOUSE
    },
];

const Hanses = {
    at(x: number, y: number) {
        return hanses.filter(h => h.location.x === x && h.location.y === y)[0];
    }
};

const Locations = {
    at(x: number, y: number) {
        const l = poi.filter(h => h.location.x === x && h.location.y === y)[0];
        if (l) {
            return l;
        } else {
            return {
                name: 'Nothing'
            };
        }
    }
};

export default class World extends Phaser.State {

    public create(): void {
        const map = this.game.add.tilemap();

        map.addTilesetImage(Assets.Images.TilesetsCatastrophiTiles16.getName(), undefined, TILE_WIDTH, TILE_WIDTH);
        const locationLayer = map.create('locations', 10, 10, TILE_WIDTH, TILE_WIDTH);
        locationLayer.scale.set(SCALE);
        locationLayer.resizeWorld();


        for (let x = 0; x < 10; x++) {
            for (let y = 0; y < 10; y++) {
                map.putTile(rand(TILES_EMPTY), x, y, locationLayer);
            }
        }

        poi.forEach(p => {
            map.putTile(p.tile, p.location.x, p.location.y, locationLayer);
        });

        const hansLayer = map.createBlankLayer('hanses', 10, 10, TILE_WIDTH, TILE_WIDTH);
        hansLayer.scale.set(SCALE);

        hanses.forEach(hans => {
            map.putTile(TILE_HANS, hans.location.x, hans.location.y, hansLayer);
        });

        const marker = this.game.add.graphics();
        marker.lineStyle(2, 0x000000, 1);
        marker.drawRect(0, 0, TILE_WIDTH * SCALE, TILE_WIDTH * SCALE);

        function updateMarker() {

            const tileX = hansLayer.getTileX(this.game.input.activePointer.worldX);
            const tileY = hansLayer.getTileY(this.game.input.activePointer.worldY);
            marker.x = tileX * TILE_WIDTH;
            marker.y = tileY * TILE_WIDTH;

            updateInfo(tileX, tileY);

            // if (game.input.mousePointer.isDown)
            // {
            //     map.putTile(currentTile, currentLayer.getTileX(marker.x), currentLayer.getTileY(marker.y), currentLayer);
            //     // map.fill(currentTile, currentLayer.getTileX(marker.x), currentLayer.getTileY(marker.y), 4, 4, currentLayer);
            // }

        }

        this.game.input.addMoveCallback(updateMarker, this);

        const bounds = new Phaser.Rectangle(TILE_WIDTH * SCALE * 10, 0, 200, TILE_WIDTH * SCALE * 10);
        const graphics = this.game.add.graphics(bounds.x, bounds.y);
        graphics.beginFill(0x000077);
        graphics.drawRect(0, 0, bounds.width, bounds.height);

        const infoText = this.game.add.text(bounds.x, bounds.y, 'info:');

        function updateInfo(x, y) {
            const hans = Hanses.at(x, y);
            if (hans) {
                infoText.text = `${hans.name}\n${hans.face}`;
                return;
            }

            const location = Locations.at(x, y);
            if (location) {
                infoText.text = location.name;
                return;
            }

            infoText.text = '???';
        }


    }
}
