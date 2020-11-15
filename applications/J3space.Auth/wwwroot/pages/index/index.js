const time = document.getElementById('time'),
    greeting = document.getElementById('greeting');

function showTime() {
  let today = new Date(),
      hour = today.getHours(),
      min = today.getMinutes(),
      sec = today.getSeconds();

  time.innerHTML = `${hour}<span>:</span>${addZero(min)}<span>:</span>${addZero(sec)}`;

  setTimeout(showTime, 1000);
}

function addZero(n) {
  return (parseInt(n, 10) < 10 ? '0' : '') + n;
}

function setBgGreet() {
  let today = new Date(),
      hour = today.getHours();

  if (greeting) {
    greeting.textContent = hour < 12
        ? 'Good Morning, '
        : hour < 18
            ? 'Good Afternoon, '
            : 'Good Evening, ';
  }

}

showTime();
setBgGreet();
