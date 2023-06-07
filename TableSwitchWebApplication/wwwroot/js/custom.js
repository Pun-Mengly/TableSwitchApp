function saveInLocalStorage(ADUser, Role, UserID) {
    localStorage.setItem('ADUser', ADUser   );
    localStorage.setItem('Role', Role);
    localStorage.setItem('UserID', UserID);
}
function showAlert(message) {
    alert(message);
}