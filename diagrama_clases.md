# Diagrama de Clases consolidado por Entidad - Clinicks

Este diagrama agrupa los atributos (campos de base de datos) y simplifica los métodos asociados de cada entidad, mostrando principalmente los de la capa de negocio (Servicios) y los métodos únicos de otras capas (Controlador/Repositorio) que no tienen equivalencia directa en la capa de negocio. Además, incorpora la abstracción y concreción del patrón de diseño **State** para gestionar los estados del turno de forma dinámica.

```mermaid
%%{init: { "themeCSS": ".classGroup text { font-size: 22pt !important; font-weight: bold !important; }" } }%%
classDiagram
    class Paciente {
        %% Atributos / Campos de Base de Datos
        +int IdPaciente
        +string Nombre
        +string Apellido
        +DateOnly FechaNacimiento
        +string Dni
        +string? Correo
        +string? Observaciones
        +DateTime? FechaRegistracion
        +EstadoPaciente Estado
        +Direccion Direccion
        
        +ObtenerTodos() 
        +ObtenerPorId(id: int) 
        +ObtenerAtendidosPorMedico(medicoId: int, search: string) 
        +ObtenerPorDni(dni: string) 
        +ValidarPaciente(dni: string) 
        +ObtenerHistorialClinico(pacienteId: int) 
    }

    class Turno {
        +int IdTurno
        +Paciente Paciente
        +Medico Medico
        +TurnoState EstadoActual
        +DateTime FechaTurno
        +DateTime? FechaRegistracion
        +string? Motivo
        +ConsultaMedica? Consulta
        +Procedimiento? Procedimiento
        
        +ObtenerTurnosMedico(idMedico: int, fechaInicio: DateTime?, fechaFin: DateTime?) 
        +ObtenerTurnoPorId(idTurno: int) 
        +CancelarTurnosVencidos() 
        +ObtenerParaActualizar(idTurno: int) 
        +ObtenerTurnoPendienteDelDia(idPaciente: int, idMedico: int, fecha: DateTime) 
        +CrearTurno(turno: Turno) 
        +ActualizarTurno(turno: Turno) 
        +ObtenerIdEstadoPorNombre(nombre: string) 
        +ObtenerIdsEstadosPorNombres(nombres: List~string~) 
        +ObtenerTurnosPorFechaYEstados(fechaLimite: DateTime, estadosIds: List~int~) 
        +ActualizarLoteTurnos(turnos: List~Turno~) 
        +CambiarEstado(nuevoEstado: TurnoState, nuevoEstadoId: int)
        +Confirmar()
        +IniciarAtencion()
        +FinalizarAtencion()
        +Cancelar()
    }

    class ConsultaMedica {
        +int IdConsulta
        +Paciente Paciente
        +Medico Medico
        +string Motivo
        +string Diagnostico
        +string? Tratamiento
        +string? Observacion
        +string? Recomendacion
        +DateTime FechaConsulta
        
        +RegistrarConsulta(consulta: ConsultaAltaDto, idMedico: int)
        +ListaConsultas()
        +ObtenerHistorialConsultas(pacienteId: int)
    }

    class Procedimiento {
        +int IdProcedimiento
        +string Tipo
        +string? Descripcion
        +DateTime Fecha
        +string? Resultado
        
        +RegistrarProcedimiento(procedimiento: ProcedimientoAltaDto, idMedico: int)
        +ObtenerTiposProceso()
        +ObtenerHistorialProcedimientos(pacienteId: int)
    }

    class Usuario {
        +int IdUsuario
        +string Nombre
        +string Apellido
        +string Correo
        +string Username
        +string Password
        +EstadoUsuario Estado
        
        +IniciarSesion(request: LoginRequest)
        +EncriptarClaves(secretKey: string)
        +Autenticar(username: string, password: string)
        +ObtenerUsuarioPorUsername(username: string)
        +ObtenerUsuarioPorMatricula(matricula: string)
        +ActualizarUsuario(usuario: Usuario)
    }

    class Medico {
        +int IdMedico
        +string Nombre
        +string Apellido
        +string Correo
        +string Dni
        +string Matricula
        +Direccion Direccion
        +Especialidad Especialidad
        +Usuario? Usuario
        
        +ObtenerMedicoPorUsuarioId(usuarioId: int) 
    }

    class TurnoState {
        <<abstract>>
        +Confirmar(turno: Turno)
        +IniciarAtencion(turno: Turno)
        +FinalizarAtencion(turno: Turno)
        +Cancelar(turno: Turno)
    }

    class TurnoPendiente {
        +Confirmar(turno: Turno)
        +FinalizarAtencion(turno: Turno)
        +Cancelar(turno: Turno)
    }

    class TurnoConfirmado {
        +IniciarAtencion(turno: Turno)
        +FinalizarAtencion(turno: Turno)
        +Cancelar(turno: Turno)
    }

    class TurnoEnCurso {
        +FinalizarAtencion(turno: Turno)
    }

    class TurnoAtendido {
    }

    class TurnoCancelado {
    }

    class TurnoStateFactory {
        <<utility>>
        +CrearEstado(idEstadoTurno: int) TurnoState
    }

    class EstadoPaciente {
        <<enumeration>>
        Activo
        Inactivo
    }

    class EstadoUsuario {
        <<enumeration>>
        Activo
        Inactivo
    }

    class Especialidad {
        +int IdEspecialidad
        +string Nombre
    }

    class Direccion {
        +int IdDireccion
        +string NombreCalle
        +int NumeroCalle
        +int IdCodigoPostal
    }

    Usuario "1" --> "0..1" Medico : tiene
    Medico "1" --> "0..*" Turno : atiende
    Paciente "1" --> "0..*" Turno : solicita
    Paciente "1" --> "0..*" ConsultaMedica : recibe
    Medico "1" --> "0..*" ConsultaMedica : realiza
    Turno "0..1" --> "0..1" ConsultaMedica : genera
    Turno "0..1" --> "0..1" Procedimiento : asocia
    
    Turno "1" --> "1" TurnoState : tiene
    TurnoState <|-- TurnoPendiente
    TurnoState <|-- TurnoConfirmado
    TurnoState <|-- TurnoEnCurso
    TurnoState <|-- TurnoAtendido
    TurnoState <|-- TurnoCancelado

    Turno ..> TurnoStateFactory : usa
    TurnoStateFactory ..> TurnoState : crea

    Paciente "0..*" --> "1" EstadoPaciente : estado
    Paciente "0..*" --> "1" Direccion : direccion
    Usuario "0..*" --> "1" EstadoUsuario : estado
    Medico "0..*" --> "1" Especialidad : especialidad
    Medico "0..*" --> "1" Direccion : direccion
```
