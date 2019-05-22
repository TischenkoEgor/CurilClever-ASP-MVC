// функция переключения в реэим для печати
function SwitchToPrintView() {
  //запомним этот режим в куки (используется библиотека jquery.cookie.js)
  // в куку с именем print_view поставвим значение 1
  $.cookie('print_view', '1');

  // отключаем отображение меню 
  $('header').css('display', 'none');
  // отключаем отображение панели с названием страницы
  $('.jumbotron').css('display', 'none')

  // показываем кнопку переключкения в режим для печати
  $('#print_view_btn').css('display', 'none');
  // и прячем  кнопку переключкения в обычный режим
  $('#default_view_btn').css('display', 'block');

}
function SwitchToDefaultView() {
  $.cookie('print_view', 'null');

  $('header').css('display', 'block');
  $('.jumbotron').css('display', 'block')
  $('.container').css('max-width', '')

  // прячем кнопку переключкения в режим для печати
  $('#print_view_btn').css('display', 'block');
  // и показываем  кнопку переключкения в обычный режим 
  $('#default_view_btn').css('display', 'none');
}