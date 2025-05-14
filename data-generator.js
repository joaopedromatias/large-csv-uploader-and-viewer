const fs = require('fs');
const path = require('path');

const outputPath = path.join(__dirname, 'data.csv');
const stream = fs.createWriteStream(outputPath, { encoding: 'utf8' });

const generateRandomName = () => {
  const letters = 'abcdefghijklmnopqrstuvwxyz';
  let nameArr = []
  for (let i = 0; i < 20; i++) {
    nameArr.push(letters[Math.floor(Math.random() * letters.length)]);
  }
  return nameArr.join('');
};

const generateRandomPrice = () => {
  return (Math.random() * 9 + 1).toFixed(2);
};

const expiration = '10/10/2023';

stream.write('name;price;expiration\n');

let i = 1_000_000;
function write() {
  while (i > 0) {
    i--;
    const line = `${generateRandomName()};${generateRandomPrice()};${expiration}\n`;
    if (i === 0) {
      stream.write(line, () => stream.end());
      break
    } else {
      stream.write(line);
    }
  }
}

write();

stream.on('finish', () => {
  console.log('CSV generated');
});
