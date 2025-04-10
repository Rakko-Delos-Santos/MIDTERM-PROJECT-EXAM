const API_URL = '/api/sections';
let nextTempId = -1;

document.addEventListener('DOMContentLoaded', () => {
    loadSections();

    document.getElementById('addSectionForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        addSection();
    });
});

async function loadSections() {
    try {
        const response = await fetch(API_URL);
        if (!response.ok) throw new Error('Failed to load sections');

        const sections = await response.json();
        renderSections(sections);
    } catch (error) {
        console.error('Error loading sections:', error);
        showAlert('Failed to load sections.', 'error');
    }
}

function renderSections(sections) {
    const tableBody = document.querySelector('#sectionsTable tbody');
    tableBody.innerHTML = '';

    if (sections.length === 0) {
        const tr = document.createElement('tr');
        tr.classList.add('no-data');
        tr.innerHTML = '<td colspan="5">No sections found</td>';
        tableBody.appendChild(tr);
        return;
    }

    sections.forEach(section => renderSectionRow(section));
}

function renderSectionRow(section, isTemp = false) {
    const tableBody = document.querySelector('#sectionsTable tbody');
    const placeholderRow = tableBody.querySelector('.no-data');
    if (placeholderRow) placeholderRow.remove();

    const tr = document.createElement('tr');
    tr.id = `section-${section.id}`;

    tr.innerHTML = `
    <td>${section.id}</td>
    <td>${section.sectionName}</td>
    <td>${section.subjectId || '—'}</td>
    <td>${section.firstName || ''} ${section.lastName || ''}</td>
    <td>
      ${isTemp
            ? '<span style="color:#aaa;">Saving...</span>'
            : `<button class="delete-btn" onclick="deleteSection(${section.id})">Delete</button>`}
    </td>
  `;

    tableBody.appendChild(tr);
}

function addSection() {
    const sectionName = document.getElementById('sectionName').value.trim();
    const firstName = document.getElementById('firstName').value.trim();
    const lastName = document.getElementById('lastName').value.trim();

    if (!sectionName) {
        showAlert('Section name is required.', 'error');
        return;
    }

    const tempId = nextTempId--;
    const tempSection = {
        id: tempId,
        sectionName,
        firstName,
        lastName,
        subjectId: null
    };

    renderSectionRow(tempSection, true);
    document.getElementById('addSectionForm').reset();

    fetch(API_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(tempSection)
    })
        .then(res => {
            if (!res.ok) throw new Error('Failed to save');
            return res.json();
        })
        .then(savedSection => {
            document.getElementById(`section-${tempId}`)?.remove();
            renderSectionRow(savedSection);
            showAlert('Section saved!', 'success');
        })
        .catch(err => {
            console.error(err);
            showAlert('Could not save section.', 'error');
        });
}

function deleteSection(id) {
    fetch(`${API_URL}/${id}`, { method: 'DELETE' })
        .then(res => {
            if (!res.ok) throw new Error('Failed to delete');
            document.getElementById(`section-${id}`)?.remove();
            showAlert('Section deleted.', 'success');
        })
        .catch(err => {
            console.error(err);
            showAlert('Could not delete section.', 'error');
        });
}

function showAlert(message, type) {
    const alertBox = document.createElement('div');
    alertBox.className = `alert ${type}`;
    alertBox.textContent = message;
    document.body.appendChild(alertBox);
    setTimeout(() => alertBox.remove(), 3000);
}
