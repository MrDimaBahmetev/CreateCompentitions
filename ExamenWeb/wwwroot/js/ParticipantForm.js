var participant = document.querySelector('#participant')
var creator = document.querySelector('#creator')
setTimeout(function () { register.checked = true }, 1000)
setTimeout(function () { signin.checked = true }, 2000)
function def() {
    participant.checked = true;
    creator.checked = false;
}
def()