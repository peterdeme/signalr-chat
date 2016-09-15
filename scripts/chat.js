$(function () {

        $('#dropDown1').on('click', function (event) {
            $(this).parent().toggleClass('open');
        });

        $('body').on('click', function (e) {
            if (!$('#dropDown1').is(e.target)
                && $('#dropDown1').has(e.target).length === 0
                && $('.open').has(e.target).length === 0
            ) {
                $('#dropDownDiv').removeClass('open');
            }
        });
        
        var hasPartner = false;

        setInterval(getPartner, 2000);
        function getPartner() {
            if (hasPartner === false) {
                chatHubProxy.server.getFreeUser();
            }
        }
        var partnerName = 'youpartner';
        function getPartnerName() {
            return partnerName === null ? 'yourpartner' : partnerName;
        }

        function appendSelfMessage(message) {
            $("#chatwindow").append(
          '<li class="right clearfix"> \
                <span class ="chat-img pull-right"> \
                    <img src="http://placehold.it/50/55C1E7/fff&text=ME" alt="User Avatar" class ="img-circle" /> \
                </span> \
                <div class="chat-body clearfix"> \
                    <div class="header"> \
                        <small class="text-muted"><span class="glyphicon glyphicon-time"></span><span data-livestamp="' + new Date().toISOString() + '"></span>\
                        </small> \
                    <strong class="pull-right primary-font">Me</strong> \
                        </small> \
                    </div> \
                    <p> \
                        ' + message + ' \
                    </p> \
                </div> \
            </li> \
            ')
        };

        function appendPartnerMessage(message) {
            $("#chatwindow").append(
            '<li class="left clearfix"> \
                <span class ="chat-img pull-left"> \
                    <img src="http://placehold.it/50/55C1E7/fff&text=' + getPartnerName().charAt(0) + '" alt="User Avatar" class ="img-circle" /> \
                </span> \
                <div class="chat-body clearfix"> \
                    <div class="header"> \
                        <strong class="primary-font">' + getPartnerName() + '</strong> <small class="pull-right text-muted"> \
                            <span class="glyphicon glyphicon-time"></span><span data-livestamp="' + new Date().toISOString() + '"></span>\
                        </small> \
                    </div> \
                    <p> \
                        ' + message + ' \
                    </p> \
                </div> \
            </li> \
            ')
        };

        function appendInfoMessage(message) {
            $("#chatwindow").append(
          '<li class="center clearfix"> \
                <div class="chat-body clearfix"> \
                    <div class="header"> \
                        <strong class="primary-font"></strong> <small class="pull-center text-muted"> \
                            <span class="glyphicon glyphicon-info-sign"></span>' + message + '\
                        </small> \
                    </div> \
                </div> \
            </li> \
            ')

        };

        var chatHubProxy = $.connection.mainHub;
        //Implementing client side functions

        chatHubProxy.client.addChatMessage = function (message) {
            $("#errorMessage").html('');
            appendPartnerMessage(message);
            var d = $('#panel-body'); d.scrollTop(d[0].scrollHeight - d.height());

        };

        chatHubProxy.client.addInfoMessage = function (message) {
                $("#infomessage").html(message)            
        };

        chatHubProxy.client.addPartnerDisconnectedMessage = function (message) {
            hasPartner = false;
            partnerName = null;
            appendInfoMessage(message);
            var d = $('#panel-body'); d.scrollTop(d[0].scrollHeight - d.height());
        };

        chatHubProxy.client.showError = function (message) {
            $("#errorMessage").html(message)
        };

        chatHubProxy.client.sendNumber = function (message) {
            $("#allConnections").html("Current chat users: " + message);
        }

        chatHubProxy.client.setNewPartner = function (username) {
            if (username !== null && username !== '') {
                hasPartner = true;
                partnerName = username;
                appendInfoMessage("You are now paired with " + username);
            }
        };

        chatHubProxy.client.partnerNameChanged = function (newname) {
            appendInfoMessage("Your partner has change his/her name to " + newname);
            partnerName = newname;
        };

        $.connection.hub.start().done(function () {
            $('#btn-chat').click(function () {               
                var message = $('#btn-input').val();
                if (message !== null) {
                    chatHubProxy.server.sendMessage(message);
                    appendSelfMessage(message);
                    var d = $('#panel-body'); d.scrollTop(d[0].scrollHeight - d.height());
                    $('#btn-input').val('').focus();
                }
            });
            $('#btn-input').keypress(function (e) {
                if (e.which == 13 && $('#btn-input').val() !== null) {
                    $('#btn-chat').click();
                }
            });

            $('#text-requiredUserName').keypress(function (e) {
                if (e.which == 13) {
                    $('#btn-requiredName').click();
                }
            });

            $("#text-requiredUserName").keypress(function (e) {
                var newName = $('#text-requiredUserName').val();
                if (e.which == 13 && newName !== null && newName !== '') {
                   // $('#text-requiredUserName').removeAttr('placeholder');
                    chatHubProxy.server.setUserName(newName);
                    $('#dropDownDiv').removeClass('open');
                    $('#text-requiredUserName').val('');
                }
            });
        });
    });