# Script para probar el endpoint de cursos más populares

# Primer paso: ejecutar la aplicación en segundo plano
Write-Host "Iniciando la aplicación LearnHub..." -ForegroundColor Green
Start-Process powershell -ArgumentList "-Command", "cd 'C:\Repos\DemoIA\learn-hub-back\src\Host'; dotnet run" -WindowStyle Minimized

# Esperar unos segundos para que la aplicación se inicie
Write-Host "Esperando que la aplicación se inicie..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Probar el endpoint
Write-Host "Probando el endpoint /api/course/popular..." -ForegroundColor Green

try {
    # Hacer la solicitud HTTP
    $response = Invoke-RestMethod -Uri "https://localhost:7154/api/course/popular" -Method GET -ContentType "application/json" -SkipCertificateCheck
    
    Write-Host "✅ Respuesta exitosa:" -ForegroundColor Green
    $response | ConvertTo-Json -Depth 3 | Write-Host
    
} catch {
    Write-Host "❌ Error en la solicitud:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    
    # Intentar con HTTP en lugar de HTTPS
    try {
        Write-Host "Intentando con HTTP..." -ForegroundColor Yellow
        $response = Invoke-RestMethod -Uri "http://localhost:5000/api/course/popular" -Method GET -ContentType "application/json"
        
        Write-Host "✅ Respuesta exitosa con HTTP:" -ForegroundColor Green
        $response | ConvertTo-Json -Depth 3 | Write-Host
        
    } catch {
        Write-Host "❌ Error también con HTTP:" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
    }
}

Write-Host "`nPrueba completada. Presiona cualquier tecla para continuar..." -ForegroundColor Cyan
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
