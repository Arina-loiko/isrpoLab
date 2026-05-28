const API = 'http://localhost:5000/api';

let students = [];
let subjects = [];
let labWorks = [];
let editingId = null;
let currentForm = null;

async function fetchJSON(url, options = {}) {
    const res = await fetch(url, {
        headers: { 'Content-Type': 'application/json' },
        ...options
    });
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    if (res.status === 204) return null;
    return res.json();
}

function showSection(name) {
    document.querySelectorAll('.section').forEach(s => s.classList.add('hidden'));
    document.querySelectorAll('.nav-btn').forEach(b => b.classList.remove('active'));
    document.getElementById(`section-${name}`).classList.remove('hidden');
    event.currentTarget.classList.add('active');

    if (name === 'students') loadStudents();
    else if (name === 'subjects') loadSubjects();
    else loadLabWorks();
}

async function loadStudents() {
    students = await fetchJSON(`${API}/students`);
    renderStudents();
    populateStudentFilter();
}

async function loadSubjects() {
    subjects = await fetchJSON(`${API}/subjects`);
    renderSubjects();
}

async function loadLabWorks() {
    const studentId = document.getElementById('filter-student').value;
    const url = studentId ? `${API}/labworks/student/${studentId}` : `${API}/labworks`;
    labWorks = await fetchJSON(url);
    if (!students.length) students = await fetchJSON(`${API}/students`);
    if (!subjects.length) subjects = await fetchJSON(`${API}/subjects`);
    filterLabWorks();
}

function filterLabWorks() {
    const status = document.getElementById('filter-status').value;
    const filtered = status ? labWorks.filter(l => l.status === status) : labWorks;
    renderLabWorks(filtered);
}

function populateStudentFilter() {
    const sel = document.getElementById('filter-student');
    const current = sel.value;
    sel.innerHTML = '<option value="">Все студенты</option>';
    students.forEach(s => {
        const opt = document.createElement('option');
        opt.value = s.id;
        opt.textContent = `${s.lastName} ${s.firstName}`;
        if (String(s.id) === current) opt.selected = true;
        sel.appendChild(opt);
    });
}

function statusBadge(status) {
    const map = {
        'Сдана': 'status-submitted',
        'Не сдана': 'status-not-submitted',
        'На доработке': 'status-revision'
    };
    return `<span class="status-badge ${map[status] || ''}">${status}</span>`;
}

function renderLabWorks(data) {
    const tbody = document.getElementById('labworks-body');
    if (!data.length) {
        tbody.innerHTML = '<tr><td colspan="8" style="text-align:center;color:#999;padding:20px">Нет записей</td></tr>';
        return;
    }
    tbody.innerHTML = data.map(l => {
        const student = l.student || students.find(s => s.id === l.studentId);
        const subject = l.subject || subjects.find(s => s.id === l.subjectId);
        const studentName = student ? `${student.lastName} ${student.firstName}` : '—';
        const subjectName = subject ? subject.name : '—';
        const date = l.submittedDate ? new Date(l.submittedDate).toLocaleDateString('ru-RU') : '—';
        const grade = l.grade != null ? `<span class="grade-badge">${l.grade}</span>` : '—';
        return `<tr>
            <td>${studentName}</td>
            <td>${subjectName}</td>
            <td>${l.labNumber}</td>
            <td>${l.title}</td>
            <td>${statusBadge(l.status)}</td>
            <td>${grade}</td>
            <td>${date}</td>
            <td>
                <button class="btn-edit" onclick="editLabWork(${l.id})">Изменить</button>
                <button class="btn-delete" onclick="deleteLabWork(${l.id})">Удалить</button>
            </td>
        </tr>`;
    }).join('');
}

function renderStudents() {
    const tbody = document.getElementById('students-body');
    if (!students.length) {
        tbody.innerHTML = '<tr><td colspan="5" style="text-align:center;color:#999;padding:20px">Нет записей</td></tr>';
        return;
    }
    tbody.innerHTML = students.map(s => `<tr>
        <td>${s.lastName}</td>
        <td>${s.firstName}</td>
        <td>${s.middleName}</td>
        <td>${s.group}</td>
        <td>
            <button class="btn-edit" onclick="editStudent(${s.id})">Изменить</button>
            <button class="btn-delete" onclick="deleteStudent(${s.id})">Удалить</button>
        </td>
    </tr>`).join('');
}

function renderSubjects() {
    const tbody = document.getElementById('subjects-body');
    if (!subjects.length) {
        tbody.innerHTML = '<tr><td colspan="3" style="text-align:center;color:#999;padding:20px">Нет записей</td></tr>';
        return;
    }
    tbody.innerHTML = subjects.map(s => `<tr>
        <td>${s.name}</td>
        <td>${s.teacherName}</td>
        <td>
            <button class="btn-edit" onclick="editSubject(${s.id})">Изменить</button>
            <button class="btn-delete" onclick="deleteSubject(${s.id})">Удалить</button>
        </td>
    </tr>`).join('');
}

function openModal(title) {
    document.getElementById('modal-title').textContent = title;
    document.getElementById('modal-overlay').classList.remove('hidden');
}

function closeModal(e) {
    if (e && e.target !== document.getElementById('modal-overlay')) return;
    document.getElementById('modal-overlay').classList.add('hidden');
    editingId = null;
    currentForm = null;
}

function openStudentModal(student = null) {
    editingId = student ? student.id : null;
    currentForm = 'student';
    openModal(student ? 'Редактировать студента' : 'Добавить студента');
    document.getElementById('modal-body').innerHTML = `
        <div class="form-group"><label>Фамилия</label><input id="f-lastName" value="${student?.lastName || ''}"></div>
        <div class="form-group"><label>Имя</label><input id="f-firstName" value="${student?.firstName || ''}"></div>
        <div class="form-group"><label>Отчество</label><input id="f-middleName" value="${student?.middleName || ''}"></div>
        <div class="form-group"><label>Группа</label><input id="f-group" value="${student?.group || ''}"></div>
        <div class="form-actions">
            <button class="btn-cancel" onclick="closeModal()">Отмена</button>
            <button class="btn-primary" onclick="saveStudent()">Сохранить</button>
        </div>`;
}

function openSubjectModal(subject = null) {
    editingId = subject ? subject.id : null;
    currentForm = 'subject';
    openModal(subject ? 'Редактировать дисциплину' : 'Добавить дисциплину');
    document.getElementById('modal-body').innerHTML = `
        <div class="form-group"><label>Название</label><input id="f-name" value="${subject?.name || ''}"></div>
        <div class="form-group"><label>Преподаватель</label><input id="f-teacherName" value="${subject?.teacherName || ''}"></div>
        <div class="form-actions">
            <button class="btn-cancel" onclick="closeModal()">Отмена</button>
            <button class="btn-primary" onclick="saveSubject()">Сохранить</button>
        </div>`;
}

function openLabWorkModal(lw = null) {
    editingId = lw ? lw.id : null;
    currentForm = 'labwork';
    openModal(lw ? 'Редактировать лабораторную работу' : 'Добавить лабораторную работу');

    const studentOptions = students.map(s =>
        `<option value="${s.id}" ${lw?.studentId === s.id ? 'selected' : ''}>${s.lastName} ${s.firstName}</option>`
    ).join('');
    const subjectOptions = subjects.map(s =>
        `<option value="${s.id}" ${lw?.subjectId === s.id ? 'selected' : ''}>${s.name}</option>`
    ).join('');
    const statuses = ['Не сдана', 'Сдана', 'На доработке'];
    const statusOptions = statuses.map(s =>
        `<option value="${s}" ${(lw?.status || 'Не сдана') === s ? 'selected' : ''}>${s}</option>`
    ).join('');
    const date = lw?.submittedDate ? lw.submittedDate.split('T')[0] : '';

    document.getElementById('modal-body').innerHTML = `
        <div class="form-group"><label>Студент</label><select id="f-studentId">${studentOptions}</select></div>
        <div class="form-group"><label>Дисциплина</label><select id="f-subjectId">${subjectOptions}</select></div>
        <div class="form-group"><label>Номер работы</label><input id="f-labNumber" type="number" min="1" value="${lw?.labNumber || 1}"></div>
        <div class="form-group"><label>Название</label><input id="f-title" value="${lw?.title || ''}"></div>
        <div class="form-group"><label>Статус</label><select id="f-status">${statusOptions}</select></div>
        <div class="form-group"><label>Оценка (1–10)</label><input id="f-grade" type="number" min="1" max="10" value="${lw?.grade ?? ''}"></div>
        <div class="form-group"><label>Дата сдачи</label><input id="f-submittedDate" type="date" value="${date}"></div>
        <div class="form-group"><label>Примечания</label><textarea id="f-notes" rows="2">${lw?.notes || ''}</textarea></div>
        <div class="form-actions">
            <button class="btn-cancel" onclick="closeModal()">Отмена</button>
            <button class="btn-primary" onclick="saveLabWork()">Сохранить</button>
        </div>`;
}

function val(id) { return document.getElementById(id)?.value.trim(); }

async function saveStudent() {
    const data = {
        lastName: val('f-lastName'),
        firstName: val('f-firstName'),
        middleName: val('f-middleName'),
        group: val('f-group')
    };
    if (editingId) {
        await fetchJSON(`${API}/students/${editingId}`, { method: 'PUT', body: JSON.stringify(data) });
    } else {
        await fetchJSON(`${API}/students`, { method: 'POST', body: JSON.stringify(data) });
    }
    document.getElementById('modal-overlay').classList.add('hidden');
    await loadStudents();
}

async function saveSubject() {
    const data = { name: val('f-name'), teacherName: val('f-teacherName') };
    if (editingId) {
        await fetchJSON(`${API}/subjects/${editingId}`, { method: 'PUT', body: JSON.stringify(data) });
    } else {
        await fetchJSON(`${API}/subjects`, { method: 'POST', body: JSON.stringify(data) });
    }
    document.getElementById('modal-overlay').classList.add('hidden');
    await loadSubjects();
}

async function saveLabWork() {
    const gradeVal = val('f-grade');
    const dateVal = val('f-submittedDate');
    const data = {
        studentId: parseInt(val('f-studentId')),
        subjectId: parseInt(val('f-subjectId')),
        labNumber: parseInt(val('f-labNumber')),
        title: val('f-title'),
        status: val('f-status'),
        grade: gradeVal ? parseInt(gradeVal) : null,
        submittedDate: dateVal || null,
        notes: val('f-notes')
    };
    if (editingId) {
        await fetchJSON(`${API}/labworks/${editingId}`, { method: 'PUT', body: JSON.stringify(data) });
    } else {
        await fetchJSON(`${API}/labworks`, { method: 'POST', body: JSON.stringify(data) });
    }
    document.getElementById('modal-overlay').classList.add('hidden');
    await loadLabWorks();
}

async function editStudent(id) {
    const s = students.find(x => x.id === id);
    openStudentModal(s);
}

async function editSubject(id) {
    const s = subjects.find(x => x.id === id);
    openSubjectModal(s);
}

async function editLabWork(id) {
    const lw = labWorks.find(x => x.id === id);
    openLabWorkModal(lw);
}

async function deleteStudent(id) {
    if (!confirm('Удалить студента?')) return;
    await fetchJSON(`${API}/students/${id}`, { method: 'DELETE' });
    await loadStudents();
}

async function deleteSubject(id) {
    if (!confirm('Удалить дисциплину?')) return;
    await fetchJSON(`${API}/subjects/${id}`, { method: 'DELETE' });
    await loadSubjects();
}

async function deleteLabWork(id) {
    if (!confirm('Удалить запись?')) return;
    await fetchJSON(`${API}/labworks/${id}`, { method: 'DELETE' });
    await loadLabWorks();
}

(async function init() {
    students = await fetchJSON(`${API}/students`);
    subjects = await fetchJSON(`${API}/subjects`);
    populateStudentFilter();
    await loadLabWorks();
})();
