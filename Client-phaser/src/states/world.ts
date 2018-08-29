import * as Assets from '../assets';
import {Backend} from '../utils/backend';



const rand = myArray => myArray[Math.floor(Math.random() * myArray.length)];
const TILE_HOUSE = 4;
const TILES_EMPTY = [0, 3, 5];
const TILE_HANS = 11;

const SCALE = 1;
const TILE_WIDTH = 16;

export default class World extends Phaser.State {

    public create(): void {
        const map = this.game.add.tilemap();
        const mapBounds = new Phaser.Rectangle(0, 0, 10 * TILE_WIDTH, 10 * TILE_WIDTH);

        map.addTilesetImage(Assets.Images.TilesetsCatastrophiTiles16.getName(), undefined, TILE_WIDTH, TILE_WIDTH);
        const locationLayer = map.create('locations', 10, 10, TILE_WIDTH, TILE_WIDTH);
        locationLayer.scale.set(SCALE);
        locationLayer.resizeWorld();


        for (let x = 0; x < 10; x++) {
            for (let y = 0; y < 10; y++) {
                map.putTile(rand(TILES_EMPTY), x, y, locationLayer);
            }
        }

        Backend.locations.forEach(p => {
            map.putTile(TILE_HOUSE, p.coordinates.x, p.coordinates.y, locationLayer);
        });

        const hansLayer = map.createBlankLayer('hanses', 10, 10, TILE_WIDTH, TILE_WIDTH);
        hansLayer.scale.set(SCALE);

        Backend.hanses.forEach(hans => {
            map.putTile(TILE_HANS, hans.location.coordinates.x, hans.location.coordinates.y, hansLayer);
        });

        const marker = this.game.add.graphics();
        marker.lineStyle(2, 0x000000, 1);
        marker.drawRect(0, 0, TILE_WIDTH * SCALE, TILE_WIDTH * SCALE);

        function updateMarker() {
            const x = this.game.input.activePointer.worldX;
            const y = this.game.input.activePointer.worldY;

            if (mapBounds.contains(x, y)) {
                const tileX = hansLayer.getTileX(x);
                const tileY = hansLayer.getTileY(y);
                marker.x = tileX * TILE_WIDTH;
                marker.y = tileY * TILE_WIDTH;


                if (this.game.input.mousePointer.isDown) {
                    updateInfo(tileX, tileY);
                }

            }

        }

        this.game.input.addMoveCallback(updateMarker, this);

        // const bounds = new Phaser.Rectangle(TILE_WIDTH * SCALE * 10, 0, 200, TILE_WIDTH * SCALE * 10);
        const infoBoxHans = new InfoBox(this.game, TILE_WIDTH * SCALE * 10, 0);
        const infoBoxLocation = new InfoBox(this.game, TILE_WIDTH * SCALE * 10 + 200, 0);


        function updateInfo(x, y) {
            const hans = Backend.hansAt(x, y);
            if (hans) {
                infoBoxHans.displayHans(hans);
            }

            const location = Backend.locationAt(x, y);
            if (location) {
                infoBoxLocation.displayLocation(location);
            }
        }

        // FIXME ticking does not update the content of the infoBoxes
        const tickAction = () => {
            Backend.tick().then(() => {
                Backend.loadHanses();
                Backend.loadLocations();
            });
        };

        const tickButton = this.game.add.button(0, TILE_WIDTH * SCALE * 10, Assets.Spritesheets.SpritesheetsButtonHorizontal64322.getName(), tickAction, this, 1, 0);
        const tickButtonText = this.game.add.text(tickButton.x, tickButton.y, 'tick', {fill: 'white'});


    }
}

class InfoBox {
    private infoText: Phaser.Text;
    private buttons: Phaser.Button[] = [];
    private texts: Phaser.Text[] = [];
    private bounds: Phaser.Rectangle;

    constructor(private game, private x, private y) {
        this.bounds = new Phaser.Rectangle(x, y, 200, TILE_WIDTH * SCALE * 10);
        const graphics = this.game.add.graphics(this.bounds.x, this.bounds.y);
        graphics.beginFill(0x00483B);
        graphics.drawRect(0, 0, this.bounds.width, this.bounds.height);

        this.infoText = this.game.add.text(this.bounds.x, this.bounds.y, '...', {fontSize: '12px'});
    }

    clear() {
        this.infoText.setText('');
        this.buttons.forEach(b => b.destroy());
        this.buttons = [];
        this.texts.forEach(t => t.destroy());
        this.texts = [];
    }

    displayHans(hans) {
        this.clear();
        let text = `${hans.name}\no,O\n` + Object.entries(hans.stats).map(([stat, value]) => `${stat}: ${value}`).join('\n');
        this.infoText.setText(text);
    }

    displayLocation(location) {
        this.clear();
        this.infoText.setText(location.name);
        const locationId = Backend.locations.indexOf(location);
        location.activities.forEach(a => {
            const callback = () => {
                console.log('queue activity', a);
                Backend.queueActivity(0, locationId, a);
            };
            this.addButton(a, callback);
        });
    }

    addButton(text, callback) {
        const buttonHeight = 32;
        const x = this.bounds.x;
        const y = this.bounds.y + this.infoText.height + this.buttons.length * buttonHeight;
        const button = this.game.add.button(x, y, Assets.Spritesheets.SpritesheetsButtonHorizontal64322.getName(), callback, this, 1, 0);
        this.buttons.push(button);

        const buttonText = this.game.add.text(x, y, text);
        this.texts.push(buttonText);
    }
}