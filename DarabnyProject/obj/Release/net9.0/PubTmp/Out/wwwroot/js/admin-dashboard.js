document.addEventListener('DOMContentLoaded', function () {
    if (typeof syncAuthNav === 'function') syncAuthNav();

    // ── Storage keys ────────────────────────────────────────────────
    const KEY = {
        courses:     'adminCourses',
        categories:  'adminCategories',
        chapters:    'adminChapters',
        certs:       'adminCertificates'
    };

    // ── Helpers ──────────────────────────────────────────────────────
    function load(key) {
        try { return JSON.parse(localStorage.getItem(key) || '[]'); } catch { return []; }
    }
    function save(key, data) { localStorage.setItem(key, JSON.stringify(data)); }
    function uid() { return Date.now() + '-' + Math.random().toString(36).slice(2, 7); }

    // ── State ────────────────────────────────────────────────────────
    let courses    = load(KEY.courses);
    let categories = load(KEY.categories);
    let chapters   = load(KEY.chapters);
    let certs      = load(KEY.certs);

    // ── Stats ────────────────────────────────────────────────────────
    function updateStats() {
        document.getElementById('statCourseCount').textContent   = courses.length;
        document.getElementById('statCategoryCount').textContent = categories.length;
        document.getElementById('statCertCount').textContent     = certs.length;
    }

    // ════════════════════════════════════════════════════════════════
    //  SHARED: populate dropdowns across sections
    // ════════════════════════════════════════════════════════════════
    function populateCategoryDropdown(selectEl, selectedId) {
        if (!selectEl) return;
        selectEl.innerHTML = '<option value="">-- Select Category --</option>';
        categories.forEach(function (cat) {
            const o = document.createElement('option');
            o.value = cat.id; o.textContent = cat.name;
            if (selectedId && selectedId === cat.id) o.selected = true;
            selectEl.appendChild(o);
        });
    }

    function populateCertDropdown(selectEl, selectedId) {
        if (!selectEl) return;
        selectEl.innerHTML = '<option value="">-- No Certificate --</option>';
        certs.forEach(function (cert) {
            const o = document.createElement('option');
            o.value = cert.id; o.textContent = cert.name;
            if (selectedId && selectedId === cert.id) o.selected = true;
            selectEl.appendChild(o);
        });
    }

    function populateCourseDropdown(selectEl, selectedId) {
        if (!selectEl) return;
        selectEl.innerHTML = '<option value="">-- Select Course --</option>';
        courses.forEach(function (c) {
            const o = document.createElement('option');
            o.value = c.id; o.textContent = c.title;
            if (selectedId && selectedId === c.id) o.selected = true;
            selectEl.appendChild(o);
        });
    }

    function getCatName(id)  { const c = categories.find(function(x){ return x.id===id; }); return c ? c.name : id||'-'; }
    function getCertName(id) { const c = certs.find(function(x){ return x.id===id; }); return c ? c.name : '—'; }
    function getCourseName(id) { const c = courses.find(function(x){ return x.id===id; }); return c ? c.title : id||'-'; }

    // ════════════════════════════════════════════════════════════════
    //  COURSES
    // ════════════════════════════════════════════════════════════════
    const courseForm      = document.getElementById('adminCourseForm');
    const courseFormTitle = document.getElementById('adminFormTitle');
    const courseIdInput   = document.getElementById('adminCourseId');
    const titleInput      = document.getElementById('adminTitleInput');
    const catInput        = document.getElementById('adminCategoryInput');
    const levelInput      = document.getElementById('adminLevelInput');
    const priceInput      = document.getElementById('adminPriceInput');
    const instrInput      = document.getElementById('adminInstructorInput');
    const imageInput      = document.getElementById('adminImageInput');
    const certCourseInput = document.getElementById('adminCertInput');
    const descInput       = document.getElementById('adminDescriptionInput');
    const reqInput        = document.getElementById('adminRequirementsInput');
    const learnInput      = document.getElementById('adminWhatLearnInput');
    const saveCourseBtn   = document.getElementById('adminSaveBtn');
    const cancelCourseBtn = document.getElementById('adminCancelEditBtn');
    const courseTableBody = document.getElementById('adminCoursesTableBody');
    const courseStatus    = document.getElementById('adminStatusText');

    function renderCourses() {
        if (!courses.length) {
            courseTableBody.innerHTML = '<tr><td colspan="4" class="text-center py-3">No courses yet.</td></tr>';
            courseStatus.textContent = '0 courses';
            updateStats();
            return;
        }
        courseTableBody.innerHTML = courses.map(function (c) {
            return '<tr>' +
                '<td>' + c.title + '</td>' +
                '<td>' + getCatName(c.categoryId) + '</td>' +
                '<td>EGP ' + c.price + '</td>' +
                '<td><div class="admin-actions">' +
                    '<button class="btn-outline-modern admin-edit-course-btn" type="button" data-id="' + c.id + '">Edit</button>' +
                    '<button class="btn-outline-modern admin-delete-btn" type="button" data-id="' + c.id + '">Delete</button>' +
                    '<a class="btn-outline-modern" href="/Home/CourseDetails">View</a>' +
                '</div></td>' +
            '</tr>';
        }).join('');
        courseStatus.textContent = courses.length + ' course(s)';
        updateStats();
    }

    // ── Inline Chapter Builder (inside course form) ──────────────────
    const chapBuilder     = document.getElementById('courseChaptersBuilder');
    const addChapterBtn   = document.getElementById('addCourseChapterBtn');

    function makeInlineLessonRow(title, url) {
        const d = document.createElement('div');
        d.className = 'lesson-row lesson-row-inline';
        d.innerHTML =
            '<input class="form-control" type="text" placeholder="Lesson title" value="' + (title||'') + '">' +
            '<input class="form-control" type="url"  placeholder="Video URL (optional)" value="' + (url||'') + '">' +
            '<button class="btn-outline-modern admin-remove-lesson-btn" type="button" title="Remove lesson">✕</button>';
        return d;
    }

    function makeInlineChapterBlock(chapData) {
        const wrap = document.createElement('div');
        wrap.className = 'inline-chapter-block';

        const header = document.createElement('div');
        header.className = 'inline-chapter-header';
        header.innerHTML =
            '<input class="form-control inline-chap-title" type="text" placeholder="Chapter title" value="' + ((chapData && chapData.title)||'') + '" required>' +
            '<input class="form-control inline-chap-order" type="number" min="1" placeholder="Order" value="' + ((chapData && chapData.order)||'1') + '" style="max-width:80px">' +
            '<button class="btn-outline-modern admin-remove-chap-btn" type="button" title="Remove chapter">Remove Chapter</button>';

        const lessonsWrap = document.createElement('div');
        lessonsWrap.className = 'inline-lessons-wrap';

        const lessons = (chapData && chapData.lessons) ? chapData.lessons : [];
        if (lessons.length) {
            lessons.forEach(function(l){ lessonsWrap.appendChild(makeInlineLessonRow(l.title, l.url)); });
        } else {
            lessonsWrap.appendChild(makeInlineLessonRow('', ''));
        }

        const addLessonBtn = document.createElement('button');
        addLessonBtn.className = 'btn-outline-modern mt-1 mb-2';
        addLessonBtn.type = 'button';
        addLessonBtn.innerHTML = '<i class="fa-solid fa-plus me-1"></i>Add Lesson';
        addLessonBtn.addEventListener('click', function() {
            lessonsWrap.appendChild(makeInlineLessonRow('', ''));
        });

        lessonsWrap.addEventListener('click', function(e) {
            if (e.target.closest('.admin-remove-lesson-btn')) {
                const rows = lessonsWrap.querySelectorAll('.lesson-row-inline');
                if (rows.length > 1) e.target.closest('.lesson-row-inline').remove();
            }
        });

        wrap.appendChild(header);
        wrap.appendChild(lessonsWrap);
        wrap.appendChild(addLessonBtn);

        wrap.querySelector('.admin-remove-chap-btn').addEventListener('click', function() {
            wrap.remove();
        });

        return wrap;
    }

    addChapterBtn.addEventListener('click', function() {
        chapBuilder.appendChild(makeInlineChapterBlock(null));
    });

    function getChaptersFromBuilder() {
        return Array.from(chapBuilder.querySelectorAll('.inline-chapter-block')).map(function(block) {
            const title  = block.querySelector('.inline-chap-title').value.trim();
            const order  = Number(block.querySelector('.inline-chap-order').value) || 1;
            const lessons = Array.from(block.querySelectorAll('.lesson-row-inline')).map(function(row) {
                const inputs = row.querySelectorAll('input');
                return { title: inputs[0].value.trim(), url: inputs[1].value.trim() };
            }).filter(function(l){ return l.title; });
            return { title, order, lessons };
        }).filter(function(ch){ return ch.title; });
    }

    function loadChaptersToBuilder(chapList) {
        chapBuilder.innerHTML = '';
        (chapList || []).forEach(function(ch) { chapBuilder.appendChild(makeInlineChapterBlock(ch)); });
    }
    // ── END Inline Chapter Builder ────────────────────────────────────

    function resetCourseForm() {
        courseIdInput.value = '';
        courseForm.reset();
        chapBuilder.innerHTML = '';
        courseFormTitle.textContent = 'Add New Course';
        saveCourseBtn.textContent   = 'Add Course';
        cancelCourseBtn.style.display = 'none';
        populateCategoryDropdown(catInput, null);
        populateCertDropdown(certCourseInput, null);
    }

    function fillCourseForm(c) {
        courseIdInput.value    = c.id;
        titleInput.value       = c.title;
        levelInput.value       = c.level;
        priceInput.value       = c.price;
        imageInput.value       = c.image || '';
        descInput.value        = c.description;
        reqInput.value         = (c.requirements || []).join('\n');
        learnInput.value       = (c.whatWillLearn || []).join('\n');
        populateCategoryDropdown(catInput, c.categoryId);
        populateCertDropdown(certCourseInput, c.certId || '');
        loadChaptersToBuilder(c.chapters || []);
        courseFormTitle.textContent   = 'Edit Course';
        saveCourseBtn.textContent     = 'Save Changes';
        cancelCourseBtn.style.display = 'inline-flex';
        document.getElementById('courseSection').scrollIntoView({ behavior: 'smooth' });
    }

    courseForm.addEventListener('submit', function (e) {
        e.preventDefault();
        const inlineChapters = getChaptersFromBuilder();
        const payload = {
            title:        titleInput.value.trim(),
            categoryId:   catInput.value,
            level:        levelInput.value,
            price:        Number(priceInput.value),
            instructor:   instrInput.value,
            image:        imageInput.value.trim(),
            certId:       certCourseInput.value,
            description:  descInput.value.trim(),
            requirements: reqInput.value.split('\n').map(function(s){return s.trim();}).filter(Boolean),
            whatWillLearn: learnInput.value.split('\n').map(function(s){return s.trim();}).filter(Boolean),
            chapters:     inlineChapters
        };
        if (!payload.title || !payload.description) return;
        const id = courseIdInput.value;
        if (id) {
            courses = courses.map(function(c){ return c.id===id ? Object.assign({},c,payload) : c; });
            // sync chapters array in standalone chapters store too
            chapters = chapters.filter(function(ch){ return ch.courseId !== id; });
            inlineChapters.forEach(function(ch, i) {
                chapters.push({ id: uid(), courseId: id, title: ch.title, order: ch.order, lessons: ch.lessons });
            });
        } else {
            const newCourse = Object.assign({ id: uid(), createdAt: new Date().toISOString() }, payload);
            courses.unshift(newCourse);
            // also push to standalone chapters store
            inlineChapters.forEach(function(ch) {
                chapters.push({ id: uid(), courseId: newCourse.id, title: ch.title, order: ch.order, lessons: ch.lessons });
            });
        }
        save(KEY.courses, courses);
        save(KEY.chapters, chapters);
        renderCourses();
        renderChapters();
        populateCourseDropdown(document.getElementById('adminChapCourseInput'), null);
        resetCourseForm();
    });

    courseTableBody.addEventListener('click', function (e) {
        const editBtn   = e.target.closest('.admin-edit-course-btn');
        const deleteBtn = e.target.closest('.admin-delete-btn');
        if (editBtn) {
            const c = courses.find(function(x){ return x.id===editBtn.dataset.id; });
            if (c) fillCourseForm(c);
        }
        if (deleteBtn && confirm('Delete this course?')) {
            const id = deleteBtn.dataset.id;
            courses = courses.filter(function(c){ return c.id!==id; });
            save(KEY.courses, courses);
            if (courseIdInput.value===id) resetCourseForm();
            renderCourses();
        }
    });
    cancelCourseBtn.addEventListener('click', resetCourseForm);

    // ════════════════════════════════════════════════════════════════
    //  CATEGORIES
    // ════════════════════════════════════════════════════════════════
    const catForm      = document.getElementById('adminCategoryForm');
    const catFormTitle = document.getElementById('adminCatFormTitle');
    const catIdInput   = document.getElementById('adminCatId');
    const catNameInput = document.getElementById('adminCatNameInput');
    const catDescInput = document.getElementById('adminCatDescInput');
    const catIconInput = document.getElementById('adminCatIconInput');
    const catSaveBtn   = document.getElementById('adminCatSaveBtn');
    const catCancelBtn = document.getElementById('adminCatCancelBtn');
    const catTableBody = document.getElementById('adminCatTableBody');
    const catStatus    = document.getElementById('adminCatStatusText');

    function renderCategories() {
        if (!categories.length) {
            catTableBody.innerHTML = '<tr><td colspan="3" class="text-center py-3">No categories yet.</td></tr>';
            catStatus.textContent = '0 categories';
            updateStats();
            return;
        }
        catTableBody.innerHTML = categories.map(function (cat) {
            return '<tr>' +
                '<td>' + cat.name + '</td>' +
                '<td>' + (cat.description || '-') + '</td>' +
                '<td><div class="admin-actions">' +
                    '<button class="btn-outline-modern admin-edit-cat-btn" type="button" data-id="' + cat.id + '">Edit</button>' +
                    '<button class="btn-outline-modern admin-delete-btn" type="button" data-id="' + cat.id + '">Delete</button>' +
                '</div></td>' +
            '</tr>';
        }).join('');
        catStatus.textContent = categories.length + ' category(s)';
        updateStats();
    }

    function resetCatForm() {
        catIdInput.value = '';
        catForm.reset();
        catFormTitle.textContent = 'Add New Category';
        catSaveBtn.textContent   = 'Add Category';
        catCancelBtn.style.display = 'none';
    }

    catForm.addEventListener('submit', function (e) {
        e.preventDefault();
        const payload = { name: catNameInput.value.trim(), description: catDescInput.value.trim(), iconUrl: catIconInput.value.trim() };
        if (!payload.name) return;
        const id = catIdInput.value;
        if (id) {
            categories = categories.map(function(c){ return c.id===id ? Object.assign({},c,payload) : c; });
        } else {
            categories.push(Object.assign({ id: uid() }, payload));
        }
        save(KEY.categories, categories);
        renderCategories();
        populateCategoryDropdown(catInput, null);
        resetCatForm();
    });

    catTableBody.addEventListener('click', function (e) {
        const editBtn   = e.target.closest('.admin-edit-cat-btn');
        const deleteBtn = e.target.closest('.admin-delete-btn');
        if (editBtn) {
            const cat = categories.find(function(c){ return c.id===editBtn.dataset.id; });
            if (cat) {
                catIdInput.value = cat.id;
                catNameInput.value = cat.name;
                catDescInput.value = cat.description || '';
                catIconInput.value = cat.iconUrl || '';
                catFormTitle.textContent = 'Edit Category';
                catSaveBtn.textContent   = 'Save Changes';
                catCancelBtn.style.display = 'inline-flex';
                document.getElementById('categorySection').scrollIntoView({ behavior: 'smooth' });
            }
        }
        if (deleteBtn && confirm('Delete this category?')) {
            const id = deleteBtn.dataset.id;
            categories = categories.filter(function(c){ return c.id!==id; });
            save(KEY.categories, categories);
            if (catIdInput.value===id) resetCatForm();
            renderCategories();
            populateCategoryDropdown(catInput, null);
        }
    });
    catCancelBtn.addEventListener('click', resetCatForm);

    // ════════════════════════════════════════════════════════════════
    //  CHAPTERS & LESSONS
    // ════════════════════════════════════════════════════════════════
    const chapForm      = document.getElementById('adminChapterForm');
    const chapFormTitle = document.getElementById('adminChapFormTitle');
    const chapIdInput   = document.getElementById('adminChapId');
    const chapCourseEl  = document.getElementById('adminChapCourseInput');
    const chapTitleEl   = document.getElementById('adminChapTitleInput');
    const chapOrderEl   = document.getElementById('adminChapOrderInput');
    const lessonsBuilder= document.getElementById('lessonsBuilder');
    const addLessonBtn  = document.getElementById('addLessonRowBtn');
    const chapSaveBtn   = document.getElementById('adminChapSaveBtn');
    const chapCancelBtn = document.getElementById('adminChapCancelBtn');
    const chapTableBody = document.getElementById('adminChapTableBody');
    const chapStatus    = document.getElementById('adminChapStatusText');

    function makeLessonRow(title, url) {
        const div = document.createElement('div');
        div.className = 'lesson-row';
        div.innerHTML =
            '<input class="form-control" type="text" placeholder="Lesson title" value="' + (title||'') + '">' +
            '<input class="form-control" type="url" placeholder="Video URL (optional)" value="' + (url||'') + '">' +
            '<button class="btn-outline-modern admin-remove-lesson-btn" type="button">✕</button>';
        return div;
    }

    addLessonBtn.addEventListener('click', function () {
        lessonsBuilder.appendChild(makeLessonRow('', ''));
    });

    lessonsBuilder.addEventListener('click', function (e) {
        if (e.target.closest('.admin-remove-lesson-btn')) {
            const rows = lessonsBuilder.querySelectorAll('.lesson-row');
            if (rows.length > 1) e.target.closest('.lesson-row').remove();
        }
    });

    function getLessonsFromBuilder() {
        return Array.from(lessonsBuilder.querySelectorAll('.lesson-row')).map(function (row) {
            const inputs = row.querySelectorAll('input');
            return { title: inputs[0].value.trim(), url: inputs[1].value.trim() };
        }).filter(function (l) { return l.title; });
    }

    function resetChapForm() {
        chapIdInput.value = '';
        chapForm.reset();
        lessonsBuilder.innerHTML = '';
        lessonsBuilder.appendChild(makeLessonRow('', ''));
        chapFormTitle.textContent = 'Add Chapter';
        chapSaveBtn.textContent   = 'Add Chapter';
        chapCancelBtn.style.display = 'none';
        populateCourseDropdown(chapCourseEl, null);
    }

    function renderChapters() {
        if (!chapters.length) {
            chapTableBody.innerHTML = '<tr><td colspan="5" class="text-center py-3">No chapters yet.</td></tr>';
            chapStatus.textContent = '0 chapters';
            return;
        }
        chapTableBody.innerHTML = chapters.map(function (ch) {
            return '<tr>' +
                '<td>' + ch.order + '</td>' +
                '<td>' + getCourseName(ch.courseId) + '</td>' +
                '<td>' + ch.title + '</td>' +
                '<td>' + (ch.lessons ? ch.lessons.length : 0) + ' lesson(s)</td>' +
                '<td><div class="admin-actions">' +
                    '<button class="btn-outline-modern admin-edit-chap-btn" type="button" data-id="' + ch.id + '">Edit</button>' +
                    '<button class="btn-outline-modern admin-delete-btn" type="button" data-id="' + ch.id + '">Delete</button>' +
                '</div></td>' +
            '</tr>';
        }).join('');
        chapStatus.textContent = chapters.length + ' chapter(s)';
    }

    chapForm.addEventListener('submit', function (e) {
        e.preventDefault();
        const lessons = getLessonsFromBuilder();
        const payload = {
            courseId: chapCourseEl.value,
            title:    chapTitleEl.value.trim(),
            order:    Number(chapOrderEl.value) || 1,
            lessons:  lessons
        };
        if (!payload.courseId || !payload.title) return;
        const id = chapIdInput.value;
        if (id) {
            chapters = chapters.map(function(ch){ return ch.id===id ? Object.assign({},ch,payload) : ch; });
        } else {
            chapters.push(Object.assign({ id: uid() }, payload));
        }
        // sort by course then order
        chapters.sort(function(a,b){ return a.courseId.localeCompare(b.courseId) || a.order-b.order; });
        save(KEY.chapters, chapters);
        renderChapters();
        resetChapForm();
    });

    chapTableBody.addEventListener('click', function (e) {
        const editBtn   = e.target.closest('.admin-edit-chap-btn');
        const deleteBtn = e.target.closest('.admin-delete-btn');
        if (editBtn) {
            const ch = chapters.find(function(x){ return x.id===editBtn.dataset.id; });
            if (ch) {
                chapIdInput.value = ch.id;
                chapOrderEl.value = ch.order;
                chapTitleEl.value = ch.title;
                populateCourseDropdown(chapCourseEl, ch.courseId);
                lessonsBuilder.innerHTML = '';
                (ch.lessons || []).forEach(function (l) { lessonsBuilder.appendChild(makeLessonRow(l.title, l.url)); });
                if (!lessonsBuilder.children.length) lessonsBuilder.appendChild(makeLessonRow('',''));
                chapFormTitle.textContent = 'Edit Chapter';
                chapSaveBtn.textContent   = 'Save Changes';
                chapCancelBtn.style.display = 'inline-flex';
                document.getElementById('chapterSection').scrollIntoView({ behavior: 'smooth' });
            }
        }
        if (deleteBtn && confirm('Delete this chapter?')) {
            const id = deleteBtn.dataset.id;
            chapters = chapters.filter(function(ch){ return ch.id!==id; });
            save(KEY.chapters, chapters);
            if (chapIdInput.value===id) resetChapForm();
            renderChapters();
        }
    });
    chapCancelBtn.addEventListener('click', resetChapForm);

    // ════════════════════════════════════════════════════════════════
    //  CERTIFICATES
    // ════════════════════════════════════════════════════════════════
    const certForm      = document.getElementById('adminCertForm');
    const certFormTitle = document.getElementById('adminCertFormTitle');
    const certIdInput   = document.getElementById('adminCertId');
    const certNameEl    = document.getElementById('adminCertNameInput');
    const certDescEl    = document.getElementById('adminCertDescInput');
    const certBadgeEl   = document.getElementById('adminCertBadgeInput');
    const certSaveBtn   = document.getElementById('adminCertSaveBtn');
    const certCancelBtn = document.getElementById('adminCertCancelBtn');
    const certTableBody = document.getElementById('adminCertTableBody');
    const certStatus    = document.getElementById('adminCertStatusText');

    function renderCerts() {
        if (!certs.length) {
            certTableBody.innerHTML = '<tr><td colspan="3" class="text-center py-3">No certificates yet.</td></tr>';
            certStatus.textContent = '0 certificates';
            updateStats();
            return;
        }
        certTableBody.innerHTML = certs.map(function (cert) {
            return '<tr>' +
                '<td>' + cert.name + '</td>' +
                '<td>' + (cert.description || '-') + '</td>' +
                '<td><div class="admin-actions">' +
                    '<button class="btn-outline-modern admin-edit-cert-btn" type="button" data-id="' + cert.id + '">Edit</button>' +
                    '<button class="btn-outline-modern admin-delete-btn" type="button" data-id="' + cert.id + '">Delete</button>' +
                '</div></td>' +
            '</tr>';
        }).join('');
        certStatus.textContent = certs.length + ' certificate(s)';
        updateStats();
    }

    function resetCertForm() {
        certIdInput.value = '';
        certForm.reset();
        certFormTitle.textContent = 'Add Certificate';
        certSaveBtn.textContent   = 'Add Certificate';
        certCancelBtn.style.display = 'none';
    }

    certForm.addEventListener('submit', function (e) {
        e.preventDefault();
        const payload = { name: certNameEl.value.trim(), description: certDescEl.value.trim(), badgeUrl: certBadgeEl.value.trim() };
        if (!payload.name) return;
        const id = certIdInput.value;
        if (id) {
            certs = certs.map(function(c){ return c.id===id ? Object.assign({},c,payload) : c; });
        } else {
            certs.push(Object.assign({ id: uid() }, payload));
        }
        save(KEY.certs, certs);
        renderCerts();
        populateCertDropdown(certCourseInput, null);
        resetCertForm();
    });

    certTableBody.addEventListener('click', function (e) {
        const editBtn   = e.target.closest('.admin-edit-cert-btn');
        const deleteBtn = e.target.closest('.admin-delete-btn');
        if (editBtn) {
            const cert = certs.find(function(c){ return c.id===editBtn.dataset.id; });
            if (cert) {
                certIdInput.value  = cert.id;
                certNameEl.value   = cert.name;
                certDescEl.value   = cert.description || '';
                certBadgeEl.value  = cert.badgeUrl || '';
                certFormTitle.textContent = 'Edit Certificate';
                certSaveBtn.textContent   = 'Save Changes';
                certCancelBtn.style.display = 'inline-flex';
                document.getElementById('certSection').scrollIntoView({ behavior: 'smooth' });
            }
        }
        if (deleteBtn && confirm('Delete this certificate?')) {
            const id = deleteBtn.dataset.id;
            certs = certs.filter(function(c){ return c.id!==id; });
            save(KEY.certs, certs);
            if (certIdInput.value===id) resetCertForm();
            renderCerts();
            populateCertDropdown(certCourseInput, null);
        }
    });
    certCancelBtn.addEventListener('click', resetCertForm);

    // ── Initial render ───────────────────────────────────────────────
    populateCategoryDropdown(catInput, null);
    populateCertDropdown(certCourseInput, null);
    populateCourseDropdown(chapCourseEl, null);
    renderCourses();
    renderCategories();
    renderChapters();
    renderCerts();
    updateStats();
});
