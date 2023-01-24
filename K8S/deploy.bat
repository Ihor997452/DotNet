
kubectl apply -f platform-depl.yaml
kubectl apply -f command-depl.yaml
kubectl apply -f platform-np-service.yaml
kubectl apply -f ingress-service.yaml
kubectl apply -f local-pvc.yaml
kubectl apply -f mysql-platform-depl.yaml
kubectl apply -f rabbitmq-depl.yaml