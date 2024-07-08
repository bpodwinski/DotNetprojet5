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
        const response = await fetch(url);
        const html = await response.text();

        document.getElementById('modalLabel').innerText = title; // Set modal title
        document.querySelector('#Modal .modal-body').innerHTML = html; // Set modal body

        const modal = new bootstrap.Modal(document.getElementById('Modal'));
        modal.show();

        if (selectpicker) {
            $('.selectpicker').selectpicker();
        }
    } catch (error) {
        console.error('Error loading the modal content:', error);
    }
}
