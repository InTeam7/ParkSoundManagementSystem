// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code
$(document).ready(function () {
    $('#calendar').calendar({
        ampm: false,
        type: 'time',
        onChange: function (date, text, mode) {
            console.log(text);
            hubConnection.invoke("ChangeTime", text);

        }

    });
    $('.ui.slider')
        .slider({
            min: 0,
            max: 100,
            start: 0,
            step: 1,
            smooth: true,
            onMove: function (value) {
                $('#slider_count').text(value);
                hubConnection.invoke("SetVolume", value);
            }
        });
})


$('.ui.dropdown').dropdown();

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/send")
    .build();

$('#select_computer').dropdown({
    onChange: function (value, text, $choice) {
        $('#slider').addClass('disabled');
        hubConnection.invoke("CheckIsOnlineSelectedComputer", value);
    }
});

$('body').on('click', '#delete', function () {
    $(this).closest('.message').remove();
})
$('body').on('click', '#send', function () {
    let voiceMessageName = $(this).closest('.message').attr('id');
    hubConnection.invoke("PlayVoiceMessage", voiceMessageName);
})
var oldVal = "";
$("#textarea").on('input selectionchange propertychange', function () {
    var currentVal = $(this).val();
    if (currentVal == oldVal) {
        return;
    }
    oldVal = currentVal;
    hubConnection.invoke("ChangeText", $(this).val());
    
});
$('#play_notify').click(function () {
    hubConnection.invoke("PlayNotify", "play");
})

$('#text_speech').click(function () {
    if ($("#select_voice").dropdown('get text') == 'Диктор') {
        $('#select_voice').transition('bounce');
    }
    else {
        hubConnection.invoke("SpeechText", $("#textarea").val());
    }
    
    //hubConnection.invoke("PlayNotify", "play");
    
})
$('#men_voice').click(function () {
    hubConnection.invoke("ChangeVoice", "men");
})
$('#woman_voice').click(function () {
    hubConnection.invoke("ChangeVoice", "woman");
})

$('#sound_on').click(function () {
    hubConnection.invoke("TurnOnSound", "on");
})
$('#sound_off').click(function () {
    hubConnection.invoke("TurnOffSound", "off");
})


function PostBlob(blob) {
    var formData = new FormData();
    formData.append('audio-filename', fileName);
    formData.append('audio-blob', blob);
    formData.append(fileName, fileName);
    xhr('/RecordRTC/PostRecordedAudioVideo', formData, function (fName) {
        let path = document.location.origin + '/UploadedFiles/' + fileName;
        let src = 'src = "' + path + '"';
        $("#voice").append(
            '<div class = "message" id="' + fileName + '" >' +
            '<div class="ui inverted divider"></div>' +
            '<audio ' + 'id="' + fileName + '"' + ' controls=""' + src + '></audio>' +
            '<div class="ui "></div>' +
            '<div class="ui hidden divider"></div>' +
            '<div id="send" class="ui inverted basic green right floated button">Отправить</div>' +
            '<div id="delete"' + ' class="ui delete inverted basic red button">Удалить</div>' +
            '</div>');
        var mediaElement = document.getElementById(fileName);
        mediaElement.load();

    });
}
var record = document.getElementById('record');
var stop = document.getElementById('stop');
var container = document.getElementById('voice');
var recordAudio;
record.onclick = function () {
    record.disabled = true;
    navigator.getUserMedia = navigator.getUserMedia || navigator.mozGetUserMedia || navigator.webkitGetUserMedia;
    navigator.getUserMedia({
        audio: true,
    }, function (stream) {

        recordAudio = RecordRTC(stream, {
            type: 'audio',
            recorderType: StereoAudioRecorder,
            desiredSampRate: 16000
        });
        recordAudio.startRecording();
        stop.disabled = false;
    }, function (error) {
        alert(error.toString());
    });
};
var fileName;
stop.onclick = function () {
    record.disabled = false;
    stop.disabled = true;
    fileName = (Math.round(Math.random() * 999999)) + '.wav';
    recordAudio.stopRecording(function () {
        PostBlob(recordAudio.getBlob());
    });

};

function xhr(url, data, callback) {
    var request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        if (request.readyState == 4 && request.status == 200) {
            callback(request.responseText);
        }
    };
    request.open('POST', url);
    request.send(data);
}


var count = 1;
hubConnection.on('SendCount', function (message) {
    $('#slider').addClass('disabled');
    count = Number(message);
    $('#count_number').text(count);
    $('#repeat_count').progress('set progress',count);
    
});
hubConnection.on('StatusResponce', function (volume) {
    $('.ui.slider').slider('set value', volume, fireChange = false);
    $('#slider_count').text(volume);
    $('#slider').removeClass('disabled');
    setTimeout(function () {
        $('#slider').addClass('disabled');
    }, 60000);

});
hubConnection.on('SendText', function (message) {
    $('#textarea').val(message);
})

hubConnection.on('SendTime', function (message) {
    $('#calendar').calendar('set date', message, updateInput = true, fireChange = false);
})

$('#increment_count').click(function () {
    hubConnection.invoke("IncrementCount", count);
})
$('#decrement_count').click(function () {
    hubConnection.invoke("DecrementCount", count);
})

hubConnection.on('IncrementCount', function (_count) {
    $('#repeat_count').progress('increment');
    $('#count_number').text(_count);
});
hubConnection.on('DecrementCount', function (_count) {
    $('#repeat_count').progress('decrement');
    $('#count_number').text(_count);
});
var mess;
hubConnection.on('TurnOnSound', function (mess) {
    $('#sound_off').css('border', '#737373 1px solid');
    $('#sound_on').css('border', 'green 3px solid')
});
hubConnection.on('TurnOffSound', function (mess) {
    $('#sound_on').css('border', '#737373 1px solid')
    $('#sound_off').css('border', 'red 3px solid')
});
hubConnection.on('Computers', function (computers) {
    for (var i = 0; i < computers.length; i++) {
        $('#selector_computer').append('<div class="item" data-value="'+computers[i]+'">' + computers[i]+'</div>');   
    }
});




hubConnection.start();
