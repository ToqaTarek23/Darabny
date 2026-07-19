document.addEventListener('DOMContentLoaded', function () {
    if (typeof syncAuthNav === 'function') syncAuthNav();

    const grid           = document.getElementById('coursesGrid');
    const status         = document.getElementById('coursesStatus');
    const searchInput    = document.getElementById('coursesSearchInput');
    const categorySelect = document.getElementById('coursesCategorySelect');
    const sortSelect     = document.getElementById('coursesSortSelect');

    let allCourses    = [];
    let allCategories = [];
    let activeCategory = 'all';
    let searchQuery    = '';
    let sortMode       = 'default';

    // ── Read from localStorage (set by admin-dashboard.js) ───────────
    // !! CRUD HOOK: replace these two functions with API/controller calls
    //    when server-side CRUD is wired up.
    function getAdminCourses() {
        try { return JSON.parse(localStorage.getItem('adminCourses') || '[]'); } catch { return []; }
    }

    function getAdminCategories() {
        try { return JSON.parse(localStorage.getItem('adminCategories') || '[]'); } catch { return []; }
    }
    // ── END CRUD HOOK ─────────────────────────────────────────────────

    function getCatName(id, cats) {
        const cat = cats.find(function (c) { return c.id === id; });
        return cat ? cat.name : (id || '');
    }

    // ── Populate category filter ──────────────────────────────────────
    function populateCategoryFilter(categories) {
        if (!categorySelect) return;
        // !! CRUD HOOK: categories come from getAdminCategories() above.
        //    When server-side CRUD exists, pass @ViewBag.Categories here.
        categorySelect.innerHTML = '<option value="all">All Categories</option>';
        categories.forEach(function (cat) {
            const o = document.createElement('option');
            o.value = cat.id;
            o.textContent = cat.name;
            categorySelect.appendChild(o);
        });
    }

    // ── Filter & sort ─────────────────────────────────────────────────
    function filteredCourses() {
        const q = searchQuery.trim().toLowerCase();
        let result = allCourses.filter(function (c) {
            const catName = getCatName(c.categoryId, allCategories).toLowerCase();
            const byCategory = activeCategory === 'all' || c.categoryId === activeCategory;
            const byQuery = !q || c.title.toLowerCase().includes(q) || catName.includes(q) || (c.description||'').toLowerCase().includes(q);
            return byCategory && byQuery;
        });

        if (sortMode === 'priceAsc')   result.sort(function(a,b){ return a.price - b.price; });
        if (sortMode === 'priceDesc')  result.sort(function(a,b){ return b.price - a.price; });
        if (sortMode === 'ratingDesc') result.sort(function(a,b){ return (b.rating||0) - (a.rating||0); });
        return result;
    }

    // ── Card template ─────────────────────────────────────────────────
    function courseCard(c) {
        const catName = getCatName(c.categoryId, allCategories);
        const img     = c.image
            ? '<img src="' + c.image + '" alt="' + c.title + '" loading="lazy">'
            : '<div class="course-cover-placeholder"><i class="fa-solid fa-book-open fa-2x"></i></div>';

        return '<article class="course-feature" data-id="' + c.id + '">' +
            '<div class="course-cover">' + img + '<span class="course-cover-badge"><i class="fa-solid fa-book-open"></i></span></div>' +
            '<h3>' + c.title + '</h3>' +
            '<p>' + (c.description || '') + '</p>' +
            '<div class="course-meta">' +
                '<span>' + (c.level || '') + '</span>' +
                '<span>EGP ' + (c.price || 0) + '</span>' +
                '<span>' + catName + '</span>' +
            '</div>' +
            // !! CRUD HOOK: replace href with asp-controller/action when CRUD is wired
            '<a class="btn-primary-modern d-block text-center text-decoration-none" href="/Home/CourseDetails">View Details</a>' +
        '</article>';
    }

    // ── Render ────────────────────────────────────────────────────────
    function render() {
        if (!grid) return;
        const visible = filteredCourses();

        if (!allCourses.length) {
            // !! CRUD HOOK: this empty state shows until admin adds courses via Dashboard.
            //    Once CRUD is connected, courses auto-populate here.
            grid.innerHTML =
                '<article class="courses-empty-state">' +
                    '<i class="fa-solid fa-book-open fa-3x mb-3" style="opacity:0.3"></i>' +
                    '<h3>No courses yet</h3>' +
                    '<p>Courses added from the <a href="/Dashboard/Admin">Admin Dashboard</a> will appear here.</p>' +
                '</article>';
            if (status) status.textContent = '0 courses';
            return;
        }

        if (!visible.length) {
            grid.innerHTML =
                '<article class="courses-empty-state">' +
                    '<h3>No matching courses</h3>' +
                    '<p>Try a different category or search term.</p>' +
                '</article>';
            if (status) status.textContent = '0 courses match';
            return;
        }

        grid.innerHTML = visible.map(courseCard).join('');
        if (status) status.textContent = visible.length + ' course(s) found';
    }

    // ── Bind events ───────────────────────────────────────────────────
    if (searchInput) {
        searchInput.addEventListener('input', function (e) { searchQuery = e.target.value || ''; render(); });
    }
    if (categorySelect) {
        categorySelect.addEventListener('change', function (e) { activeCategory = e.target.value || 'all'; render(); });
    }
    if (sortSelect) {
        sortSelect.addEventListener('change', function (e) { sortMode = e.target.value || 'default'; render(); });
    }

    // ── Init ──────────────────────────────────────────────────────────
    // !! CRUD HOOK: replace getAdminCourses() / getAdminCategories() with
    //    a fetch() call to your MVC controller when server-side is ready.
    allCourses    = getAdminCourses();
    allCategories = getAdminCategories();
    populateCategoryFilter(allCategories);
    render();
});
