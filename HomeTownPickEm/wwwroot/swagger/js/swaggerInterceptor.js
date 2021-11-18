// source: https://www.w3schools.com/js/js_cookies.asp

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function getAuthToken() {
    const user = JSON.parse(getCookie('user'));
    return user.token;

}

function addAuthTokenToHeader(req) {
    var token = getAuthToken();
    req.headers['Authorization'] = 'Bearer ' + token;
    return req;
}