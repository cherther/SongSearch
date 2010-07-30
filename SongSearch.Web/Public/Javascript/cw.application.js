if(!Array.prototype.indexOf){Array.prototype.indexOf=function(a){for(var b=0;b<this.length;b++){if(this[b]===a){return b}}return -1}}if(!Array.prototype.contains){Array.prototype.contains=function(a){for(var b=0;b<this.length;b++){if(this[b]===a){return true}}return false}}if(!String.prototype.contains){String.prototype.contains=function(b){return this.indexOf(b)!=-1}}if(!String.prototype.swap){String.prototype.swap=function(b,a){return this.contains(b)?this.replace(b,a):this.replace(a,b)}}var _debug=false;var _maxCart=100;var _cartOverMaxMsg="Sorry, you can only add up to "+_maxCart+" items to your cart.";var _cartAddErrorMsg="There was a problem adding the item(s) to your cart. Please make sure you have less than 100 items in your cart.";function Rnd(){return Math.floor(Math.random()*100001).toString()}var waiting=false;function wait(c,d){if(!waiting){waiting=true}}function unwait(b){if(waiting){}waiting=false}function debug(b){if(_debug){feedback("debug",b)}}function feedback(g,f){unwait();var j="Message";var h="/public/images/icons/silk/information.png";var i=3000;switch(g){case"error":j="Error";h="/public/images/icons/silk/error.png";i=8000;break;case"debug":j="Debugging";h="/public/images/icons/silk/plugin.png";i=8000;break}$.gritter.add({title:j,time:i,image:h,text:f})}function pluralize(d,c){return c==1?d:d+"s"}function setupTooltips(){$("[title]").tipTip({delay:800})}function toggleTagBoxSelection(f,e,d){f.siblings().removeClass(e);if(d){f.addClass(e)}}function millSecsToTimeCode(i,f){var h=Math.floor(i/1000);var j=Math.floor(h/60);var g=h-(j*60);return(f?(j+":"+(g<10?"0"+g:g)):{min:j,sec:g})}function toFileSizeDescription(b){return parseInt((b/(1024*1024))).toString()+" MB"}function toYesNo(b){return b?"Yes":"No"}var contentDetailPanelId="#cw-content-detail-panel";var searchOptionsPanelId="#cw-search-options-panel";var tagBoxSelectedClass="cw-blue";var inputSelectedClass="cw-input-highlight";var isContentDetailShowing;var isContentViewMode;var isContentEditMode;var isContentSaveMode;var lastContentDetailLinkClicked;var lastContentEditLinkClicked;var _currentContentDetailTab;var aCache={};function setupAutoComplete(){$(".cw-autocomplete").autocomplete({source:function(h,i){var g=$(this)[0].element[0].alt;if(g){try{if(aCache.term==h.term&&aCache.field==g&&aCache.content){i(aCache.content);return}if(new RegExp(aCache.term).test(h.term)&&aCache.content&&aCache.content.length<13){i($.ui.autocomplete.filter(aCache.content,h.term));return}}catch(j){}var f="/Search/AutoComplete?f=";$.ajax({url:f+g,dataType:"json",data:h,success:function(a){aCache.field=g;aCache.term=h.term;aCache.content=a;i(a)}})}},minLength:2})}function clearSearchForm(b){clearForm(b);$(".cw-tagbox-search").removeClass(tagBoxSelectedClass);$(".cw-form-value").removeClass(inputSelectedClass)}function clearForm(b){$(":input.cw-form-value",b).each(function(){var d=this.type;var a=this.tagName.toLowerCase();if(d=="text"||d=="password"||a=="textarea"||d=="hidden"){this.value=""}else{if(d=="checkbox"||d=="radio"){this.checked=false}else{if(a=="select"){this.selectedIndex=-1}}}})}function setSelectedSearchTagValue(h){var l=h[0].id;var i="#"+h[0].rev;var k=h[0].rel;var g=$(i)!=null?$(i).val().split(";"):null;var j=g.indexOf(k);if(j>-1){g.splice(j,1)}else{g.push(k)}$(i).val(g.join(";"));h.toggleClass(tagBoxSelectedClass)}function toggleRowCheckboxes(d){var c=$("."+d.id+":enabled");if($(d).is(":checked")){c.attr("checked","checked")}else{c.removeAttr("checked")}}function strikeoutRowCheckboxes(){var c=$(".cw-row-checkbox:checked");c.attr("disabled","disabled");var d=c.parent("td").siblings();d.css("text-decoration","line-through");d.attr("disabled","true");d.children("a").attr("disabled","true")}function removeCheckedRows(){var b=$(".cw-row-checkbox-delete:checked");b.closest("tr").remove()}function isTableBodyEmpty(c){var d=$(c+" > TBODY > TR");return(d.length==0)}function toggleContentsTable(b){if(b.length>0){b.text(b.text().swap("Show","Hide"))}$(".cw-tbl-catalog-contents").toggle();closeContentPanel()}function hideEmptyContentsTable(){var d=$(".cw-tbl-catalog-contents");var c=$("#cw-catalog-contents-show-link");if(isTableBodyEmpty(".cw-tbl-catalog-contents")){d.hide();c.hide();$("#cw-catalog-contents-msg").show()}}function updateAddToCartAllButtontext(d){var c=d>0?"Add ("+d+")":"Add";$("#cw-add-all-to-cart span").text(c)}function setUpMediaFileDialog(h){var j=$("#upload-form");var f=h[0].rel;var g=h[0].rev;$("#contentId").val(f);$("#uploadTitle").text(g);if(j){j.dialog("destroy");var i=j.html();j.dialog({autoOpen:false,height:300,width:350,modal:true,title:"Select & upload new media files",buttons:{Upload:function(){saveMediaFilesAjax("#saveMediaFilesForm",function(){feedback("info","Files uploaded");resetMediaFileDialog(i);$(this).dialog("close")})},Cancel:function(){resetMediaFileDialog(i);$(this).dialog("close")}},close:function(){resetMediaFileDialog(i)}});j.dialog("open");resetUploaders();setupMediaUploader("previewVersionUploadContainer","previewVersionUpload","previewVersionFilelist","Preview",0);setupMediaUploader("fullVersionUploadContainer","fullVersionUpload","fullVersionFilelist","Full",1)}}function resetMediaFileDialog(c){var d=$("#upload-form");if(d){d.dialog("destroy");d.html(c)}}function showContentPanel(f){var g=f[0];if(!f.is(":disabled")){isContentEditMode=g.rel=="Edit";isContentSaveMode=g.rel=="Save";isContentViewMode=!isContentEditMode&&!isContentSaveMode;if(isContentViewMode){mediaStop()}var h=lastContentDetailLinkClicked!=null?f[0]==lastContentDetailLinkClicked[0]:false;if(!h||!isContentViewMode){var e=f[0].href;getContentDetailAjax(e,f)}else{closeContentPanel();lastContentDetailLinkClicked=null}}}function saveContentPanel(d,f){var e=d[0];isContentEditMode=e.rel=="Edit";isContentSaveMode=e.rel=="Save";isContentViewMode=!isContentEditMode&&!isContentSaveMode;if(isContentSaveMode){submitContentFormAjax("#cw-content-editor",d,f)}else{closeContentPanel();lastContentDetailLinkClicked=null}}function showContentPanelCallback(j,g){if(isContentViewMode&&isContentDetailShowing){closeContentPanel()}var l=g.closest("tr");var k=6;var i=$('<tr id="cw-content-detail-row"><td colspan="'+k+'">'+j);l.addClass("cw-row-selected");l.after(i);unwait();setupContentPanelUIControls();var h=g[0].rel;if(h){mediaPlay(h,"#cw-play-full")}isContentDetailShowing=true;lastContentDetailLinkClicked=g}function showContentPanelCallbackEdit(e,f,g){if(isContentSaveMode){feedback("info","Item Saved")}lastContentEditLinkClicked=isContentSaveMode?null:f;if(g){unwait();showContentPanel(g)}else{var h=$("#cw-content-detail-data");h.html(e);unwait();setupContentPanelUIControls();f.text(f.text().swap("Edit","Save"));f.toggleClass("cw-gray").toggleClass("cw-red");f.toggleClass("cw-content-edit-link").toggleClass("cw-content-save-link");f[0].href=f[0].href.swap("Edit","Save");f[0].rel=f[0].rel.swap("Edit","Save");isContentDetailShowing=true}}function setupContentPanelUIControls(){var b=$("#cw-content-detail-tabs");b.tabs({select:function(d,a){_currentContentDetailTab=a.panel.id}});if(_currentContentDetailTab!=null){b.tabs("select","#"+_currentContentDetailTab)}setupVolumeSlider();setupAutoComplete()}function deleteContentRight(e){var f=e.closest("tr");var d=e.prev(".cw-model-action");d.val(2);f.hide().next().hide()}function deleteTagCallback(d,e){var f=e[0].rel;$("."+f).hide();e.hide()}function closeContentPanel(){if(isContentDetailShowing){var b=$("#cw-content-detail-row");if(b.length>0){b.remove()}$(".cw-row-selected").removeClass("cw-row-selected");isContentDetailShowing=false;_currentContentDetailTab=null;mediaStop()}}var _lastMediaButtonPressed;function mediaPlay(d,c){soundPlay(d,false);togglePlayButton(c);$(".cw-media-repeat-link").removeAttr("disabled");$(".cw-media-skip-link").removeAttr("disabled");_lastMediaButtonPressed=c}function mediaRepeat(){if(soundPlayRepeat()!=sm_ps_playing){togglePlayButton(_lastMediaButtonPressed)}}function mediaFastForward(){fastForward()}function mediaRewind(){rewind()}function mediaCue(d){var c=d.length>0?d[0]:45*1000;cue(c)}function enableRewind(b){if(b){$(".cw-media-rew-link").removeAttr("disabled")}else{$(".cw-media-rew-link").attr("disabled",true)}}function enableFastForward(b){if(b){$(".cw-media-ffwd-link").removeAttr("disabled")}else{$(".cw-media-ffwd-link").attr("disabled",true)}}function togglePlayButton(b){togglePlayButtons(b,true)}function togglePlayButtons(o,l){var j="b-play";var q="b-pause";var n="cw-gray";var m="cw-green";var k=$(o);var p=k.children("span");if(!l){if(k.hasClass(m)){k.removeClass(m).addClass(n);p.removeClass(q).addClass(j)}}else{k.toggleClass(n);k.toggleClass(m);p.toggleClass(q);p.toggleClass(j);var r=k.siblings(".cw-media-play-link");if(r.length>0){togglePlayButtons("#"+r[0].id,false)}}}function toggleAllPlayButtons(){var b=$(".cw-media-play-link");b.each(function(d){var a=b[d];togglePlayButtons("#"+a.id,false)})}function setCurrentMediaTime(c){var d=millSecsToTimeCode(c,true);$("#cw-media-player-time").html(d)}function setCurrentPosition(d,c){c=c==0?1000:c;$("#cw-media-position").width((((d/c)*100)+"%"))}function setCurrentLoadPercentage(d,c){c=c==0?1000:c;$("#cw-media-loaded").width((((d/c)*100)+"%"))}function setTotalMediaLength(c){var d=millSecsToTimeCode(c,true);$("#cw-media-player-length").html(d)}function setupVolumeSlider(){var b=_currentVolume!=null&&_currentVolume>=0?_currentVolume:60;$("#cw-media-player-volume").slider({value:b,orientation:"horizontal",range:"min",animate:true,slide:function(d,a){changeVolume(a.value)}})}var isUserDetailShowing=false;var isCatalogDetailShowing=false;var catalogDetailId="#cw-catalog-detail";var userDetailId="#cw-user-detail";function showUserDetailPanel(c){var d=$(userDetailId);d.html(c);if(!isUserDetailShowing){d.show();isUserDetailShowing=true}}function showCatalogDetailPanel(c){var d=$(catalogDetailId);d.html(c);if(!isCatalogDetailShowing){d.show();isCatalogDetailShowing=true}};
$(document).ready(function(){setupAutoComplete();setupTooltips();$.prettyLoader({loader:"/public/images/prettyLoader/ajax-loader.gif"});$(".signin").click(function(b){b.preventDefault();$("fieldset#signin_menu").toggle();$(".signin").toggleClass("menu-open")});$("fieldset#signin_menu").mouseup(function(){return false});$(document).mouseup(function(b){if($(b.target).parent("a.signin").length==0){$(".signin").removeClass("menu-open");$("fieldset#signin_menu").hide()}});$(".cw-reset-form").click(function(b){b.preventDefault();clearSearchForm($(this).parents("form"))});$(".cw-tagbox-search").click(function(d){d.preventDefault();var c=$(this);setSelectedSearchTagValue(c)});$(".cw-tagbox-label-edit").live("click",function(b){$(this).toggleClass("cw-blue")});$(".cw-tags-more-link").click(function(f){f.preventDefault();var e=$(this);var d=e.siblings(".cw-more-tags");d.toggleClass("cw-optional");e.text(e.text().swap("more","less"))});$("#cw-select-all-items-check").click(function(j){var i=$(".add-to-cart-checkbox");if($(this).is(":checked")){var g=getCartCountAjax();var h=i.filter(":not(:checked) :not(:disabled)");if(h.length+g>100){feedback("error",_cartOverMaxMsg)}else{h.attr("checked","checked");h.addClass("clicked")}}else{var f=i.filter(".clicked");f.filter(".clicked").removeAttr("checked");f.removeClass("clicked")}updateAddToCartAllButtontext($(".clicked").length)});$(".cw-select-all-items-check").live("click",function(b){toggleRowCheckboxes(this)});$("#cw-remove-all-from-cart").click(function(f){f.preventDefault();var e=$(".cw-row-checkbox-cart-remove:checked");if(e.length>0){var d=$("#cw-cart-form");d.attr("action","/Cart/RemoveMultiple");d.submit()}});$("#cw-add-all-to-cart").click(function(h){h.preventDefault();var g=$(".add-to-cart-checkbox").filter(".clicked");if(g.length>0){var f=getCartCountAjax();if(g.length+f>100){feedback("error",_cartOverMaxMsg)}else{var e=new Array();g.each(function(b){var a=g[b].id;e.push(a)});g.attr("disabled","disabled");g.removeClass("clicked");updateAddToCartAllButtontext(0);addToCartMultipleAjax($(this),e)}}});$(".add-to-cart-checkbox").click(function(f){$(this).toggleClass("clicked");var d=$(".clicked").length;var e=getCartCountAjax();if(d+e>100){feedback("error",_cartOverMaxMsg);$(this).removeClass("clicked");f.preventDefault()}else{updateAddToCartAllButtontext(d)}});$(".cw-content-detail-link").live("click",function(d){d.preventDefault();var c=$(this);if(isContentEditMode){$("#dialog-confirm-save-changes").dialog({resizable:false,height:200,width:360,modal:true,title:"Save Changes?",buttons:{"Save Changes":function(){saveContentPanel(lastContentEditLinkClicked,c);$(this).dialog("close")},Cancel:function(){showContentPanel(c);$(this).dialog("close")}}})}else{showContentPanel(c)}});$(".cw-content-edit-link").live("click",function(d){d.preventDefault();var c=$(this);showContentPanel(c)});$(".cw-delete-right-link").live("click",function(d){d.preventDefault();var c=$(this);deleteContentRight(c)});$(".cw-delete-tag-link").live("click",function(d){d.preventDefault();var c=$(this);deleteTagAjax(c)});$(".cw-content-save-link").live("click",function(d){d.preventDefault();var c=$(this);saveContentPanel(c,null)});$("#cw-write-id3-link").live("click",function(d){d.preventDefault();var c=$(this);writeID3Ajax(c)});$("#cw-detail-close-link").live("click",function(b){closeContentPanel()});$(".cw-cart-add-link").live("click",function(f){f.preventDefault();var e=$(this);var d=getCartCountAjax();if(d+1>100){feedback("error",_cartOverMaxMsg)}else{addToCartAjax(e)}});$(".cw-media-play-link").live("click",function(d){d.preventDefault();var c=this.href;mediaPlay(c,"#"+this.id)});$(".cw-media-repeat-link").live("click",function(b){b.preventDefault();mediaRepeat()});$(".cw-media-rew-link").live("click",function(b){b.preventDefault();mediaRewind()});$(".cw-media-ffwd-link").live("click",function(b){b.preventDefault();mediaFastForward()});$(".cw-media-cue-link").live("click",function(d){var c=this.rel.split(":");d.preventDefault();mediaCue(c)});$(".cw-carts-contents").click(function(d){d.preventDefault();var c=$(this)[0].id.replace("s-","");$("#c-"+c).toggle();$(this).toggleClass("cw-carts-contents-show");$(this).toggleClass("cw-carts-contents-hide")});$(".cw-user-detail-link").live("click",function(h){h.preventDefault();var f=$(this);var e=f[0].href;var g=f.parents(".cw-user-listing");$(".cw-user-listing").removeClass("cw-selected");g.addClass("cw-selected");getUserDetailAjax(e)});$("input[name='cw-system-access']").live("click",function(d){var c=$(this);if(c.is(":checked")){setSystemAdminAccessAjax(c)}});$("#cw-user-delete-link").live("click",function(b){b.preventDefault();$("#dialog-confirm-user-delete").dialog({resizable:false,height:280,width:360,modal:true,title:"Delete User?",buttons:{"Delete User":function(){$("#cw-user-delete-form").submit();$(this).dialog("close")},Cancel:function(){b.preventDefault();$(this).dialog("close")}}})});$("#cw-user-takeowner-link").live("click",function(b){b.preventDefault();$("#dialog-confirm-user-takeowner").dialog({resizable:false,height:280,width:360,modal:true,title:"Take Ownership?",buttons:{"Take Ownership":function(){$("#cw-user-takeowner-form").submit();$(this).dialog("close")},Cancel:function(){b.preventDefault();$(this).dialog("close")}}})});$(".cw-usrcat-role-edit").live("click",function(d){d.preventDefault();var c=$(this);updateUserCatRoleAjax(c)});$(".cw-usrcat-role-edit-all").live("click",function(d){d.preventDefault();var c=$(this);updateUserCatRoleAjax(c)});$(".cw-cat-role-edit").live("click",function(d){d.preventDefault();var c=$(this);updateCatRoleAjax(c)});$(".cw-cat-role-edit-all").live("click",function(d){d.preventDefault();var c=$(this);updateCatRoleAjax(c)});$("#cw-catalog-delete-link").live("click",function(b){if(!confirm("Are you sure you want to delete this catalog and all of its songs?")){b.preventDefault()}});$(".cw-catalog-detail-link").live("click",function(h){h.preventDefault();var f=$(this);var e=f[0].href;var g=f.parents(".cw-catalog-listing");$(".cw-catalog-listing").removeClass("cw-selected");g.addClass("cw-selected");closeContentPanel();getCatalogDetailAjax(e)});$("#cw-catalog-contents-show-link").live("click",function(b){b.preventDefault();toggleContentsTable($(this))});$("#cw-delete-multiple-content").live("click",function(f){f.preventDefault();var d=$(this);var e=$(".cw-row-checkbox-delete:checked");if(e.length>0){$("#dialog-confirm-song-delete").dialog({resizable:false,height:200,width:360,modal:true,title:"Delete Songs?",buttons:{"Delete Songs":function(){var a=new Array();e.each(function(c){var b=e[c].value;a.push(b)});e.attr("disabled","disabled");deleteContentMultipleAjax(d,a);$(this).dialog("close")},Cancel:function(){$(this).dialog("close")}}})}});$(".cw-media-upload-link").live("click",function(d){d.preventDefault();var c=$(this);closeContentPanel();setUpMediaFileDialog(c)})});
function getContentDetailAjax(d,c){wait();$.ajax({url:d,type:"GET",cache:false,dataType:"html",success:function(b,f,a){if(f=="error"){unwait();feedback("error",a.status+" "+a.statusText)}else{if(isContentViewMode){showContentPanelCallback(b,c)}else{showContentPanelCallbackEdit(b,c,null)}}},error:function(a,b,f){unwait();feedback("error",a.status+" "+a.statusText)}})}function submitContentFormAjax(j,g,l){var i=$(j);var k=l==null;var h={beforeSubmit:function(){wait()},data:{returnData:k},success:function(a,b,c){if(b=="error"){unwait();feedback("error",c.status+" "+c.statusText)}else{showContentPanelCallbackEdit(a,g,l)}},error:function(b,a,c){unwait();feedback("error",b.status+" "+b.statusText)}};i.ajaxSubmit(h)}function updateCartCount(){var d=getCartCountAjax();var c=$("#menu-cart a");c.text("Song Cart ("+d+")");return d}function getCartCountAjax(){var f=$("#menu-cart a");var d=f.attr("href");d=d+"/CartCount?"+Rnd();var e=0;$.ajax({async:false,url:d,type:"GET",dataType:"json",success:function(a){e=a!=null?a:0}});return e}function addToCartAjax(c){var d=c[0].href;wait();$.ajax({url:d,type:"POST",cache:false,dataType:"json",success:function(b,f,a){if(f=="error"){feedback("error",a.status+" "+a.statusText)}else{feedback("info","1 item added to your song cart");c.fadeOut("slow",function(){c.text("In Cart");c.attr("title","In Cart");c.attr("href","/Cart");c.removeClass("cw-cart-add-link");c.fadeIn("highlight")});updateCartCount();unwait()}},error:function(a,b,f){feedback("error",a.status+" "+a.statusText)}})}function addToCartMultipleAjax(e,f){var d=e[0].href;wait();$.ajax({url:d,type:"POST",cache:false,dataType:"json",data:{items:f},success:function(h,b,c){if(b=="error"){unwait();feedback("error",_cartAddErrorMsg)}else{var a=h+" "+pluralize("item",h)+" added to your song cart";feedback("info",a);updateCartCount();unwait()}},error:function(b,c,a){unwait();feedback("error",_cartAddErrorMsg)}})}function deleteTagAjax(c){var d=c[0].href;wait();$.ajax({url:d,type:"POST",cache:false,dataType:"json",success:function(b,f,a){if(f=="error"){feedback("error",a.status+" "+a.statusText)}else{deleteTagCallback(b,c);unwait()}},error:function(a,b,f){feedback("error",a.status+" "+a.statusText)}})}function writeID3Ajax(c){var d=c[0].href;wait();c.fadeOut("slow");$.ajax({url:d,type:"POST",cache:false,dataType:"json",success:function(g,h,b){if(h=="error"){feedback("error",b.status+" "+b.statusText)}else{var a=c.text();c.text("Saving...");c.fadeOut("slow",function(){c.fadeIn("highlight");c.text(a)})}unwait()},error:function(a,b,f){feedback("error",a.status+" "+a.statusText)}})}function saveMediaFilesAjax(e,g){var h=$(e);var f={beforeSubmit:function(){wait()},success:function(a,b,c){if(b=="error"){feedback("error",c.status+" "+c.statusText)}else{g()}unwait()},error:function(c,a,b){unwait();feedback("error",c.status+" "+c.statusText)}};h.ajaxSubmit(f)}function deleteContentMultipleAjax(e,f){var d=e[0].href;wait();$.ajax({url:d,type:"POST",cache:false,dataType:"json",data:{items:f},success:function(c,a,b){if(a=="error"){unwait();feedback("error",b.status+" "+b.statusText)}else{feedback("info",c+" "+pluralize("item",c)+" deleted");updateCartCount();closeContentPanel();removeCheckedRows();hideEmptyContentsTable();unwait()}},error:function(b,c,a){unwait();feedback("error",b.status+" "+b.statusText)}})}function getUserDetailAjax(b){wait();$.ajax({url:b,cache:false,dataType:"html",success:function(e,f,a){if(f=="error"){feedback("error",a.status+" "+a.statusText)}else{showUserDetailPanel(e);unwait()}},error:function(a,e,f){feedback("error",a.status+" "+a.statusText)}})}function getCatalogDetailAjax(b){wait();$.ajax({url:b,cache:false,dataType:"html",success:function(e,f,a){if(f=="error"){feedback("error",a.status+" "+a.statusText)}else{showCatalogDetailPanel(e);unwait()}},error:function(a,e,f){feedback("error",a.status+" "+a.statusText)}})}function updateRoleAjax(c){var d=c[0].href;$.ajax({url:d,dataType:"json",type:"POST",cache:false,success:function(b,f,a){if(f=="error"){feedback("error",a.status+" "+a.statusText)}else{toggleTagBoxSelection(c,"cw-green",true)}},error:function(a,b,f){feedback("error",a.status+" "+a.statusText)}})}function setSystemAdminAccessAjax(c){var d=c[0].value;$.ajax({url:d,dataType:"json",type:"POST",cache:false,success:function(b,f,a){if(f=="error"){feedback("error",a.status+" "+a.statusText)}else{feedback("info","System access updated")}},error:function(a,b,f){feedback("error",a.status+" "+a.statusText)}})}function updateUserCatRoleAjax(e){wait();var d=e[0].href;var f=e[0].rel;$.ajax({url:d,dataType:"json",type:"POST",cache:false,success:function(c,a,b){if(a=="error"){feedback("error",b.status+" "+b.statusText)}else{getUserDetailAjax(f);unwait()}},error:function(b,c,a){feedback("error",b.status+" "+b.statusText)}})}function updateCatRoleAjax(e){wait();var d=e[0].href;var f=e[0].rel;$.ajax({url:d,dataType:"json",type:"POST",cache:false,success:function(c,a,b){if(a=="error"){feedback("error",b.status+" "+b.statusText)}else{getCatalogDetailAjax(f);unwait()}},error:function(b,c,a){feedback("error",b.status+" "+b.statusText)}})};
$(document).ready(function(){soundManager.url="/public/flash/";soundManager.flashVersion=9;soundManager.debugMode=false;soundManager.useHighPerformance=true;soundManager.onready(function(b){if(b.success){_isSoundManagerReady=true}else{feedback("error","There was an error loading our Flash sound player on your system. Please turn off any Flash blocking software while using this site.")}})});var _isSoundManagerReady=false;var _mySoundId=0;var _mySound;var _lastUrlPlayed;var _currentVolume=60;var _skipInterval=0.1;var _maxSkip=30000;var sm_rs_uninitialised=0;var sm_rs_loading=1;var sm_rs_failed_error=2;var sm_rs_loaded_success=3;var sm_ps_stopped=0;var sm_ps_playing=1;var sm_ps_paused=2;function soundPlay(b){return soundPlay(b,false)}function soundPlay(e,h){var f;var g;if(_isSoundManagerReady){if(_mySound){g=_mySound.readyState;f=_mySound.paused?sm_ps_paused:_mySound.playState;if(e==_lastUrlPlayed&&!h&&_mySound.playState==sm_ps_playing){if(_mySound.readyState!=sm_rs_loaded_success){_mySound.unload()}else{_mySound.togglePause()}}else{_mySound.unload();if(f==sm_ps_playing){_mySound.stop()}_mySound=getSound(e);_mySound.play();_lastUrlPlayed=e}}else{f=sm_ps_stopped;g=sm_rs_uninitialised;_mySound=getSound(e);_mySound.play();_lastUrlPlayed=e}}return f}function soundPlayRepeat(){var b=_lastUrlPlayed;return soundPlay(b,true,null)}function getSound(b){_currentVolume=_currentVolume!=null&&_currentVolume>=0?_currentVolume:60;return soundManager.createSound({id:"cw-sound"+_mySoundId++,url:b,stream:true,volume:_currentVolume,multiShot:false,onload:function(){setTotalMediaLength(this.durationEstimate)},onfinish:function(){toggleAllPlayButtons();setCurrentMediaTime(0);setTotalMediaLength(0);setCurrentPosition(0,1);setCurrentLoadPercentage(0,this.bytesTotal)},onstop:function(){setCurrentMediaTime(0);setTotalMediaLength(0);setCurrentPosition(0,1);setCurrentLoadPercentage(0,this.bytesTotal)},whileplaying:function(){setCurrentMediaTime(this.position);setCurrentPosition(this.position,this.durationEstimate);var a=_mySound.duration*_skipInterval;enableFastForward(this.position<(this.durationEstimate-a));enableRewind(this.position>0)},whileloading:function(){setCurrentLoadPercentage(this.bytesLoaded,this.bytesTotal)}})}function mediaStop(){if(_isSoundManagerReady){if(_mySound&&_mySound.readyState!=sm_rs_failed_error&&_mySound.playState==sm_ps_playing){_mySound.unload();_mySound.stop();_lastUrlPlayed=null}}}function changeVolume(b){if(_isSoundManagerReady){if(_mySound&&_mySound.readyState!=sm_rs_failed_error&&_mySound.playState==sm_ps_playing){_mySound.setVolume(b);_currentVolume=b}}}function fastForward(){if(_isSoundManagerReady){if(_mySound&&_mySound.readyState!=sm_rs_failed_error&&_mySound.playState==sm_ps_playing){var c=_mySound.duration*_skipInterval;c=c>_maxSkip?_maxSkip:c;var d=_mySound.position+c;if(d<_mySound.duration){_mySound.setPosition(d)}enableFastForward(_mySound.position<(_mySound.duration-c))}}}function rewind(){if(_isSoundManagerReady){if(_mySound&&_mySound.readyState!=sm_rs_failed_error&&_mySound.playState==sm_ps_playing){var c=_mySound.duration*_skipInterval;c=c>_maxSkip?_maxSkip:c;var d=_mySound.position-c;_mySound.setPosition(d>0?d:0)}}}function cue(b){if(_isSoundManagerReady){if(_mySound&&_mySound.readyState!=sm_rs_failed_error&&_mySound.playState==sm_ps_playing){_mySound.setPosition(b)}}};
$(document).ready(function(){if(uploadWidget.length>0){uploadWidget.pluploadQueue(uploadOptions);var b=uploadWidget.pluploadQueue();b.bind("Init",function(a,d){debug("Runtime: "+d.runtime)});b.bind("FilesAdded",function(a,d){debug("FilesAdded event fired")});b.bind("QueueChanged",function(f,a){debug("QueueChanged event fired");checkTotalUploadSize(f);var e=$("#fileList");e.html("");$.each(f.files,function(d,c){e.append('<input type="hidden" name="state.TempFiles['+d+']" value="'+c.name+'" />')})});b.bind("Error",function(a,d){handleError(a,d);turnStepActionButtonOn(true)});catalogUploadForm.submit(function(f){var e=$(wizardUploadWidgetId).pluploadQueue();if(e.total.uploaded==0){var a=$("#minimumFiles").val();a=a!=null?a:0;if(e.files.length>=a){if(e.files.length>0){e.bind("FileUploaded",function(d,h,c){submitUploadFormWhenFilesDone(d)});e.bind("Error",function(c,d){handleError(c,d);$.prettyLoader.hide();turnStepActionButtonOn(true)});f.preventDefault();$.prettyLoader.show();e.start();turnStepActionButtonOn(false)}}else{f.preventDefault();feedback("info","Please select at least "+a+pluralize(" file",a)+" to upload.")}}})}});var wizardUploadWidgetId="#wizardUploader";var uploadLinkClass=".cw-media-upload";var audioFileFilter={title:"Audio files",extensions:"mp3"};var uploadWidget=$(wizardUploadWidgetId);var catalogUploadForm=$("#catalogUploadForm");var catalogFormIsSubmitted=false;var uploadOptions={runtimes:"gears,html5,flash,silverlight",url:"/CatalogUpload/UserMediaUpload",max_file_size:"20mb",chunk_size:"1mb",filters:[audioFileFilter],flash_swf_url:"/Public/Flash/plupload.flash.swf",silverlight_xap_url:"/Public/Flash/plupload.silverlight.xap"};function turnStepActionButtonOn(b){if(b){$("#stepAction").removeAttr("disabled")}else{$("#stepAction").attr("disabled","true")}}function submitUploadFormWhenFilesDone(c){var d=0;$.each(c.files,function(a,b){d+=c.files[a].status==plupload.DONE});if(d==c.files.length){debug("catalogUploadForm submit at done = "+d);catalogUploadForm.submit()}}function checkTotalUploadSize(g){var k=parseInt($("#maxFiles").val());var l=parseInt($("#maxBytes").val());var i=0;var j=g.files;$.each(j,function(a,b){i+=b.size});if(j.length>k){var h="Total number of files is more than "+k+" ("+j.length+")"}else{if(i>l){var h="Total file size is more than "+toFileSizeDescription(l)+" ("+toFileSizeDescription(i)+")"}}if(h){feedback("info",h)}}function handleError(c,d){if(d!=null){switch(d.code){case plupload.FILE_SIZE_ERROR:feedback("error","Please upload audio files smaller than "+toFileSizeDescription(c.settings.max_file_size)+" only.");break;case plupload.FILE_EXTENSION_ERROR:feedback("error","Please upload audio files in MP3 format only.");break;default:feedback("error","There was an trying to upload your file ("+d.message+")")}}}var _mediaUploaders=new Array();function resetUploaders(){_mediaUploaders=new Array()}function setupMediaUploader(k,l,j,g,h){var i=new plupload.Uploader({runtimes:"gears,html5,flash,silverlight",url:"/CatalogUpload/UserMediaUpload?mediaVersion="+g,max_file_size:"20mb",chunk_size:"1mb",container:k,browse_button:l,filters:[{title:"Audio files",extensions:"mp3"}],flash_swf_url:"/Public/Flash/plupload.flash.swf",silverlight_xap_url:"/Public/Flash/plupload.silverlight.xap"});var j=$("#"+j);i.bind("Init",function(a,b){});i.bind("FilesAdded",function(a,b){$.each(b,function(c,d){debug("FilesAdded: "+d.name)})});i.bind("QueueChanged",function(d){debug("QueueChanged "+d.files.length);if(d.files.length>1){for(var a=0;a<d.files.length-1;a++){var c=d.files[a];d.removeFile(c);debug("Removed "+c.name)}}j.html("");if(d.files.length>0){var c=d.files[0];var b=c.name;j.html("<label>New file: </label><br/><label><strong>"+b+"</strong></label>");$("#uploadFiles_"+h+"_FileName").val(c.name)}d.start()});i.bind("FileUploaded",function(b,a,c){debug("FileUploaded event fired")});i.bind("StateChanged",function(b,a,c){debug("StateChanged: "+b.total.queued+" queued, "+b.total.uploaded+" uploaded")});i.bind("Error",function(a,b){j.html("");handleError(a,b)});i.bind("Init",function(a,b){debug("Runtime: "+b.runtime)});i.init();_mediaUploaders[h]=i};