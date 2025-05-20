document.addEventListener('DOMContentLoaded', function () {
    var contentDiv = document.getElementById('content');

    // Load the content of the current page into the contentDiv
    fetch('content/' + window.location.pathname.split('/').pop())
        .then(response => response.text())
        .then(data => contentDiv.innerHTML = data)
        .catch(err => console.error('Error loading content:', err));
});
