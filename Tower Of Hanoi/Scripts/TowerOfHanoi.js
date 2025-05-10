const towers = [[], [], []];
let diskCount = 3;
let moveCounter = 0;
let isGameCompleted = false; 
let draggedDisk = null; 

const diskCountSpan = document.getElementById('disk-count');
const moveCounterSpan = document.getElementById('move-counter');
const towerElements = document.querySelectorAll('.tower');
const completionMessage = document.getElementById('completion-message');
const minMovesMessage = document.getElementById('min-moves-message');

function initializeDisks() {
    towers[0] = [];
    for (let i = diskCount; i > 0; i--) {
        towers[0].push(i);
    }
    towers[1] = [];
    towers[2] = [];
    renderTowers();
    moveCounter = 0;
    moveCounterSpan.textContent = moveCounter;
    completionMessage.textContent = '';
    isGameCompleted = false; 
    updateMinMovesMessage(); 
    document.getElementById('save-score').style.display = 'none';
}

function renderTowers() {
    towerElements.forEach((towerElement, towerIndex) => {
        towerElement.innerHTML = '<div class="rod"></div>';
        towers[towerIndex].forEach((diskSize, diskIndex) => {
            const disk = document.createElement('div');
            disk.classList.add('disk', `disk-${diskSize}`);
            disk.style.width = (diskSize * 20) + 'px';
            disk.style.bottom = (diskIndex * 20) + 'px';
            disk.draggable = true;
            disk.dataset.size = diskSize;
            disk.dataset.tower = towerIndex;
            towerElement.appendChild(disk);
        });
    });

    if (towers[2].length === diskCount) {
        const minMoves = Math.pow(2, diskCount) - 1;
        if (moveCounter === minMoves) {
            completionMessage.textContent = 'Perfect! You completed the puzzle in the minimum number of moves!';
        } else {
            completionMessage.textContent = 'Congratulations! You completed the puzzle!';
        }
        isGameCompleted = true;
        document.getElementById('save-score').style.display = 'inline';
    }

    addDragAndDropHandlers();
}

document.getElementById('save-score').addEventListener('click', () => {
    const minMoves = Math.pow(2, diskCount) - 1;
    const isPerfectScore = moveCounter === minMoves;
    window.location.href = `/Game/SaveScore?moves=${moveCounter}&disks=${diskCount}&isPerfect=${isPerfectScore}`;
});


function updateMinMovesMessage() {
    const minMoves = Math.pow(2, diskCount) - 1;
    minMovesMessage.textContent = `Minimum moves required: ${minMoves}`;
}

function addDragAndDropHandlers() {
    const disks = document.querySelectorAll('.disk');
    disks.forEach(disk => {
        disk.addEventListener('dragstart', handleDragStart);
    });

    towerElements.forEach((towerElement, towerIndex) => {
        towerElement.addEventListener('dragover', handleDragOver);
        towerElement.addEventListener('drop', (event) => handleDrop(event, towerIndex));
    });
}

function handleDragStart(event) {
    if (isGameCompleted) return;
    draggedDisk = event.target;
    setTimeout(() => draggedDisk.classList.add('dragging'), 0);
    event.dataTransfer.setData('text/plain', draggedDisk.dataset.size);
}

function handleDragOver(event) {
    event.preventDefault();
}

function handleDrop(event, targetTowerIndex) {
    event.preventDefault();
    if (!draggedDisk) return;

    /*const diskSize = parseInt(draggedDisk.dataset.size, 10);*/
    const sourceTowerIndex = parseInt(draggedDisk.dataset.tower, 10);

    if (canMoveDisk(sourceTowerIndex, targetTowerIndex)) {
        moveDisk(sourceTowerIndex, targetTowerIndex);
        renderTowers();
    }

    draggedDisk.classList.remove('dragging');
    draggedDisk = null;
}

function canMoveDisk(sourceTowerIndex, targetTowerIndex) {
    const fromTower = towers[sourceTowerIndex];
    const toTower = towers[targetTowerIndex];
    return fromTower.length > 0 && (toTower.length === 0 || fromTower[fromTower.length - 1] < toTower[toTower.length - 1]);
}

function moveDisk(sourceTowerIndex, targetTowerIndex) {
    towers[targetTowerIndex].push(towers[sourceTowerIndex].pop());
    moveCounter++;
    moveCounterSpan.textContent = moveCounter;
}

document.getElementById('increase-disks').addEventListener('click', () => {
    if (diskCount < 6) { 
        diskCount++;
        diskCountSpan.textContent = diskCount;
        initializeDisks();
    }
});

document.getElementById('decrease-disks').addEventListener('click', () => {
    if (diskCount > 3) {
        diskCount--;
        diskCountSpan.textContent = diskCount;
        initializeDisks();
    }
});

document.getElementById('restart').addEventListener('click', initializeDisks);

initializeDisks();
