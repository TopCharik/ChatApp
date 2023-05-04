let peerInstance;
let mediaConnectionInstance;
let ownMediaStream;

function newPeer() {
     peerInstance = new Peer();
}

function subscribeToCalls() {
     peerInstance.on('call', (mediaConnection) => {
          const getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia;
          mediaConnectionInstance = mediaConnection

          getUserMedia({video: true, audio: false}, (mediaStream) => {
               mediaConnectionInstance.answer(mediaStream)
               ownMediaStream = mediaStream;

               mediaConnectionInstance.on('stream', function (remoteStream) {
                    remote_video.srcObject = remoteStream
                    remote_video.play();
                    my_video.srcObject = mediaStream;
                    my_video.play();
               });
          });
     });
     
     return peerInstance.id;
}

function makePeerjsCall(remotePeerId) {
     const getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia;

     getUserMedia({video: true, audio: false}, (mediaStream) => {
          mediaConnectionInstance = peerInstance.call(remotePeerId, mediaStream);
          ownMediaStream = mediaStream;

          mediaConnectionInstance.on('stream', (remoteStream) => {
               remote_video.srcObject = remoteStream
               remote_video.play();
               my_video.srcObject = mediaStream;
               my_video.play();
          });
     });
}

function endCall() {
     peerInstance.destroy();
     ownMediaStream.getTracks().forEach(function(track) {
          if (track.readyState === 'live') {
               track.stop();
          }
     });
     peerInstance = null;
     mediaConnectionInstance = null;
     ownMediaStream = null;
}