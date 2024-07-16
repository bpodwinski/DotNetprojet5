// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

async function loadModal(action, id = null || 0, title = "Modal Title", selectpicker = false) {
    if (typeof action !== 'string') {
        throw new TypeError('Expected a string for action');
    }
    if (id !== null && typeof id !== 'number') {
        throw new TypeError('Expected a number or null for id');
    }
    if (typeof title !== 'string') {
        throw new TypeError('Expected a string for title');
    }
    if (typeof selectpicker !== 'boolean') {
        throw new TypeError('Expected a boolean for selectpicker');
    }

    let url = action;
    if (id) {
        url += `/${id}`;
    }

    try {
        const response = await fetch(url, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });
        const html = await response.text();

        document.getElementById('modalLabel').innerText = title; // Set modal title
        document.querySelector('#Modal .modal-body').innerHTML = html; // Set modal body

        const modal = new bootstrap.Modal(document.getElementById('Modal'));
        modal.show();

        if (selectpicker) {
            document.querySelectorAll('.selectpicker').forEach(element => {
                new SelectPicker(element);
            });
        }

        const createForm = document.getElementById('createForm');
        if (createForm) {
            createForm.onsubmit = handleCreateFormSubmit;
        }

    } catch (error) {
        console.error('Error loading the modal content:', error);
    }
}

async function handleCreateFormSubmit(event) {
    event.preventDefault();

    const form = event.target;
    const url = form.action;
    const formData = new FormData(form);

    try {
        const response = await fetch(url, {
            method: 'POST',
            body: formData,
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });
        const html = await response.text();

        const parser = new DOMParser();
        const doc = parser.parseFromString(html, 'text/html');

        if (doc.querySelector('.validation-summary-errors') || doc.querySelector('.field-validation-error')) {
            const modalBody = document.querySelector('#Modal .modal-body');
            modalBody.innerHTML = html;

            document.getElementById('createForm').onsubmit = handleCreateFormSubmit;
        } else {
            location.reload();
        }
    } catch (error) {
        console.error('Error:', error);
    }
}
