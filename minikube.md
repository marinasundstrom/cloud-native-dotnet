minikube start
minikube addons enable default-storageclass
minikube addons enable storage-provisione
minikube dashboard --url
minikube mount $HOME:/host

minikube service mssql-svc --url 